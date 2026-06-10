using UnityEngine;
using Unity.SharpZipLib.Utils;
using System.IO;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Threading;
using Dummiesman;
using TMPro;
using UnityEditor;
using System;

public class VentricleAnimation : MonoBehaviour
{
    // serialized list to hold the meshes
    [SerializeField] public static List<Mesh> meshList;

    // serialized mesh renderer
    [SerializeField] public MeshRenderer meshRenderer;

    // serialized path for locating folder in Resources with meshes
    [SerializeField] string path;

    public GameObject panel;

    // counter for mesh loop
    int counter;

    bool exist = false;

    [SerializeField] public Slider opacitySlider;

    [SerializeField] public TMP_Text opacityText;

    [SerializeField] public TMP_InputField nameText;

    public static string[] affinesList;

    string[] predictionsList;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator DownloadAndExtractFiles(string URL) 
    {
        // Reference - https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Networking.DownloadHandlerFile.html
        var var1 = new UnityWebRequest(URL, UnityWebRequest.kHttpVerbGET);
        // Reference - https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Application-persistentDataPath.html
        // Reference - https://learn.microsoft.com/en-us/dotnet/api/system.io.path.combine?view=net-10.0
        // save files in appropriate file location for VR headset to access at runtime
        string pathZip = Path.Combine(Application.persistentDataPath, "files.zip");
        // creates a download handler that lets the downloaded files to be saved to a specific file path
        var1.downloadHandler = new DownloadHandlerFile(pathZip);
        // send the web request to download the files via the pre-signed URL
        yield return var1.SendWebRequest();
        
        if (var1.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(var1.error);
        }
        
        else
        {
            Debug.Log("File successfully downloaded and saved to " + pathZip);
            // create a folder to place extracted files
            string pathExtract = Path.Combine(Application.persistentDataPath, "ExtractedFiles");
            // Reference - adapted from ChatGPT-5 (OpenAI)
            Directory.CreateDirectory(pathExtract);
            // Reference - adapted from: https://docs.unity3d.com/Packages/com.unity.sharp-zip-lib@1.4/manual/index.html
            ZipUtility.UncompressFromZip(pathZip, null, pathExtract);
            Debug.Log("Now building the model");
            StartCoroutine(BuildModel(pathExtract));
        }
    }    

    public IEnumerator BuildModel(string pathExtract)
    {
        // extracted file in folder with case name
        string name = nameText.text;
        // removing .zip from name since it is now extracted
        if (name.EndsWith(".zip"))
        {
            // Reference - https://learn.microsoft.com/en-us/dotnet/standard/base-types/trimming
            name = name.Replace(".zip", "");
        }
        Debug.Log("File name without .zip: " + name);

        // Reference - https://learn.microsoft.com/en-us/dotnet/api/system.io.path.combine?view=net-10.0
        // different path convention based on if on Windows or a different operating system
        // want to dynamically open folder regardless of name - under currentDirectory[0]
        string[] directoryList = Directory.GetDirectories(pathExtract);
        string currentDirectory = directoryList[0];
        Debug.Log("Finding directories: " + directoryList.Length + " with name " + currentDirectory);
        // string affinesPath = Path.Combine(pathExtract, name);
        string affinesPath = Path.Combine(currentDirectory, "affines");
        // string objsPath = Path.Combine(pathExtract, name);
        string objsPath = Path.Combine(currentDirectory, "objs");
        // string pngsPath = Path.Combine(pathExtract, name);
        string pngsPath = Path.Combine(currentDirectory, "pngs");
        // string predictionsPath = Path.Combine(pathExtract, name);
        string predictionsPath = Path.Combine(currentDirectory, "predictions");
        Debug.Log(affinesPath);
        Debug.Log(predictionsPath);
        Debug.Log(pngsPath);
        Debug.Log(predictionsPath);

        // Reference - https://learn.microsoft.com/en-us/dotnet/api/system.io.directory?view=net-9.0
        // finding all files in each folder
        // affinesList is public and static for later methods to check if affines are provided or not
        try
        {
            affinesList = Directory.GetFiles(affinesPath, "*.txt");
        }
        catch (Exception e)
        {
            affinesList = Array.Empty<string>();
        }
        // affinesList = Directory.GetFiles(affinesPath, "*.txt");
        string[] objsList = Directory.GetFiles(objsPath, "*.obj");
        string[] pngsList = Directory.GetFiles(pngsPath, "*.png");
        // string[] predictionsList = Directory.GetFiles(predictionsPath, "*.png");
        try
        {
            predictionsList = Directory.GetFiles(predictionsPath, "*.png");
        }
        catch (Exception e)
        {
            predictionsList = Array.Empty<string>();
        }
        Debug.Log("Number of affines: " + affinesList.Length);
        Debug.Log("Number of objs: " + objsList.Length);
        Debug.Log("Number of pngs: " + pngsList.Length);
        Debug.Log("Number of predictions: " + predictionsList.Length);
        
        Debug.Log("Building obj files");
        // creating a new mesh list to hold the meshes for the ventricle
        meshList = new List<Mesh>();
        foreach (string obj in objsList)
        {
            // Reference - https://assetstore.unity.com/packages/tools/modeling/runtime-obj-importer-49547
            // creating a new GameObject object to load the current .obj model at runtime via OBJImporter
            GameObject gameobj = new OBJLoader().Load(obj);
            // extracting the mesh from the .obj
            Mesh mesh1 = gameobj.GetComponentInChildren<MeshFilter>().mesh;
            // Reference - https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Mesh.html
            // performing recalculations to ensure no issues from loading
            mesh1.RecalculateNormals();
            mesh1.RecalculateBounds();
            mesh1.RecalculateTangents();
            // adding extracted mesh to the mesh list
            meshList.Add(mesh1);
            // deleting the instantiated GameObject
            Destroy(gameobj);
            yield return null;
        }
        Debug.Log("Extracted meshes from objs");
        // getting the mesh renderer
        meshRenderer = GetComponent<MeshRenderer>();
        // Reference - ChatGPT-5 (OpenAI)
        // attatching a shader to the game object
        meshRenderer.material.shader = Shader.Find("Universal Render Pipeline/Unlit");
        // Reference - https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Color-ctor.html
        // Reference - https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Renderer-material.html
        // checking material for debugging
        Material mat = GetComponent<Renderer>().material;
        Debug.Log("Finding material: " + mat.name + " and shader: " + mat.shader);
        // the material used is a child variant of the parent Lit from Universal Render Pipeline (built-in with Unity)
        // the main difference is the Surface Type is set to Transparent with a Blending Mode of Alpha
        // this allows for the ability to change the opacity of the model
        meshRenderer.material.color = new Color(1.0f, 0.0f, 0.0f, 0.5f);

        // initialise first mesh:
        GetComponent<MeshFilter>().mesh = meshList[0];

        panel.GetComponent<ImageAnimation>().Build(pngsList, 1);
        
        // if predictions are provided
        if (predictionsList.Length > 0)
        {
            panel.GetComponent<ImageAnimation>().Build(predictionsList, 2); 
        }
    }


    public void Incrementer(int playCounter)
    {
        GetComponent<MeshFilter>().mesh = meshList[playCounter];
    }

    public void OpacitySliderChange()
    {
        // Reference - adapted from: https://docs.unity3d.com/540/Documentation/ScriptReference/UI.Slider-onValueChanged.html (legacy documentation)
        meshRenderer.material.color = new Color(1.0f, 0.0f, 0.0f, 0.5f);
        opacityText.text = "Opacity: " + (float) opacitySlider.value/20;
        meshRenderer.material.color = new Color(1.0f, 0.0f, 0.0f,(float) opacitySlider.value/20);
        Debug.Log("changing opacity slider valuer to: " + opacitySlider.value);
    }
}

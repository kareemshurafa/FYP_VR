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

public class VentricleAnimation : MonoBehaviour
{
    // Serialized list to hold the meshes
    [SerializeField] public static List<Mesh> meshList = new List<Mesh>();

    // Serialized mesh renderer
    [SerializeField] public MeshRenderer meshRenderer;

    // Serialized path for locating folder in Resources with meshes
    [SerializeField] string path;

    public GameObject panel;

    // Counter for mesh loop
    int counter;

    bool exist = false;

    [SerializeField] public Slider opacitySlider;

    [SerializeField] public TMP_Text opacityText;

    [SerializeField] public TMP_InputField nameText;

    public static string[] affinesList;

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
        // string url = File.ReadAllText(urlpath).Trim();
        // var urlfile = Resources.Load<TextAsset>(resourcespath);
        // string url = urlfile.ToString().Trim();
        var var1 = new UnityWebRequest(URL, UnityWebRequest.kHttpVerbGET);
        
        // Reference - https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Application-persistentDataPath.html
        // Need to save files in appropriate file location for VR headset to access at runtime
        string pathZip = Path.Combine(Application.persistentDataPath, "files1.zip");
        var1.downloadHandler = new DownloadHandlerFile(pathZip);
        yield return var1.SendWebRequest();
        
        if (var1.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(var1.error);
            // button.GetComponent<Image>().color = Color.red;
            //Thread.Sleep(5000);
        }
        
        else
        {
            Debug.Log("File successfully downloaded and saved to " + pathZip);
            // button.GetComponent<Image>().color = Color.green;
            // Thread.Sleep(3000);
            string pathExtract = Path.Combine(Application.persistentDataPath, "ExtractedFiles");
            // Reference - adapted from ChatGPT-5 (OpenAI)
            Directory.CreateDirectory(pathExtract);
            // Reference - adapted from: https://docs.unity3d.com/Packages/com.unity.sharp-zip-lib@1.4/manual/index.html
            ZipUtility.UncompressFromZip(pathZip, null, pathExtract);
            //Thread.Sleep(5000);
            Debug.Log("Now calling animation method!");

            // fix!
            //build(pathExtract);
            StartCoroutine(BuildModel(pathExtract));
        }
    }    

    public IEnumerator BuildModel(string pathExtract)
    {
        // locate files in folder
        // foreach .obj file:
        //   load into game
        //   extract mesh + add to list
        //   delete (will keep initially to test it working)

        // extracted file in folder with case name
        string name = nameText.text;
        if (name.EndsWith(".zip"))
        {
            // Reference - https://learn.microsoft.com/en-us/dotnet/standard/base-types/trimming
            name = name.Replace(".zip", "");
        }
        Debug.Log("Name string " + name);

        // Reference - https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/verbatim
        // need to indicate the string literal via @
        string affinesPath = Path.Combine(pathExtract, name + "/affines");
        string objsPath = Path.Combine(pathExtract, name + "/objs");
        string pngsPath = Path.Combine(pathExtract, name + "/pngs");
        string predictionsPath = Path.Combine(pathExtract, name + "/predictions");

        // Reference - https://learn.microsoft.com/en-us/dotnet/api/system.io.directory?view=net-9.0
        Debug.Log("Building obj files!");
        // string[] objPaths = Directory.GetFiles(pathExtract, "*.obj");
        // string[] pngPaths = Directory.GetFiles(pathExtract, "*.png");
        // string[] predictionPaths = Directory.GetFiles(pathExtract, "*prediction.png");
        affinesList = Directory.GetFiles(affinesPath, "*.txt");
        string[] objsList = Directory.GetFiles(objsPath, "*.obj");
        string[] pngsList = Directory.GetFiles(pngsPath, "*.png");
        string[] predictionsList = Directory.GetFiles(predictionsPath, "*.png");
        // Debug.Log("Number of pngs: " + pngPaths.Length);
        // Debug.Log("Number of objs: " + objPaths.Length);
        Debug.Log("Number of affines: " + affinesList.Length);
        Debug.Log("Number of objs: " + objsList.Length);
        Debug.Log("Number of pngs: " + pngsList.Length);
        Debug.Log("Number of predictions: " + predictionsList.Length);
        
        meshList = new List<Mesh>();
        foreach (string obj in objsList)
        {
            // Reference - https://assetstore.unity.com/packages/tools/modeling/runtime-obj-importer-49547
            GameObject gameobj = new OBJLoader().Load(obj);

            
            Mesh mesh1 = gameobj.GetComponentInChildren<MeshFilter>().mesh;
            // Reference - https://docs.unity3d.com/6000.2/Documentation/ScriptReference/Mesh.html
            mesh1.RecalculateNormals();
            mesh1.RecalculateBounds();
            mesh1.RecalculateTangents();
            meshList.Add(mesh1);
            Destroy(gameobj);
            yield return null;
        }
        Debug.Log("Extracted Meshes from objs!");
        // Add a MeshFilter component to display the meshes in the list
        // MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();

        Debug.Log("Now adding meshRenderer!");
        // Add a MeshRenderer component and set to red
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        meshRenderer.material.shader = Shader.Find("Universal Render Pipeline/Lit");
        // meshRenderer.material.shader = Shader.Find("Mobile/Unlit");
        // Reference - https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Color-ctor.html
        // Reference - https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Renderer-material.html
        Material mat = GetComponent<Renderer>().material;
        Debug.Log("Finding material: " + mat.name + " and shader: " + mat.shader);
        Debug.Log("Setting exist equal to true!");
        exist = true;
        // the material used is a child variant of the parent Lit from Universal Render Pipeline (built-in with Unity)
        // the main difference is the Surface Type is set to Transparent with a Blending Mode of Alpha
        // this allows for the ability to change the opacity of the model
        meshRenderer.material.color = new Color(1.0f, 0.0f, 0.0f, 0.5f);

        // initialise first mesh:
        GetComponent<MeshFilter>().mesh = meshList[0];

        
        panel.GetComponent<ImageAnimation>().build(pngsList, 1);
        
        if (predictionsList.Length > 0)
        {
            panel.GetComponent<ImageAnimation>().build(predictionsList, 2); 
        } 
    }


    public void incrementer(int playCounter)
    {
        GetComponent<MeshFilter>().mesh = meshList[playCounter];
    }

    public void opacitySliderChange()
    {
        // Reference - adapted from: https://docs.unity3d.com/540/Documentation/ScriptReference/UI.Slider-onValueChanged.html (legacy documentation)
        meshRenderer.material.color = new Color(1.0f, 0.0f, 0.0f, 0.5f);
        opacityText.text = "Opacity: " + (float) opacitySlider.value/20;
        meshRenderer.material.color = new Color(1.0f, 0.0f, 0.0f,(float) opacitySlider.value/20);
        Debug.Log("changing opacity slider valuer to: " + opacitySlider.value);
    }
}

using UnityEngine;
using System.IO;
using System.Collections.Generic;
using Dummiesman;

public class VentricleAnimation : MonoBehaviour
{
    // Serialized list to hold the meshes
    [SerializeField] public List<Mesh> meshList = new List<Mesh>();

    // Serialized mesh renderer
    [SerializeField] public MeshRenderer meshRenderer;

    // Serialized path for locating folder in Resources with meshes
    [SerializeField] string path;

    public GameObject panel;

    // Counter for mesh loop
    int counter;

    bool exist = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void build(string pathExtract)
    {
        // locate files in folder
        // foreach .obj file:
        //   load into game
        //   extract mesh + add to list
        //   delete (will keep initially to test it working)
        // Reference - https://learn.microsoft.com/en-us/dotnet/api/system.io.directory?view=net-9.0
        Debug.Log("Building obj files!");
        string[] objPaths = Directory.GetFiles(pathExtract, "*.obj");
        string[] pngPaths = Directory.GetFiles(pathExtract, "*.png");
        Debug.Log("Number of pngs: " + pngPaths.Length);
        Debug.Log("Number of objs: " + objPaths.Length);
        meshList = new List<Mesh>();
        foreach (string obj in objPaths)
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
        }
        Debug.Log("Extracted Meshes from objs!");
        // Add a MeshFilter component to display the meshes in the list
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();

        Debug.Log("Now adding meshRenderer!");
        // Add a MeshRenderer component and set to red
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        // meshRenderer.material.shader = Shader.Find("Universal Render Pipeline/Lit");
        // Reference - https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Color-ctor.html
        meshRenderer.material.color = new Color(1.0f, 0.0f, 0.0f, 0.5f);
        // Reference - https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Renderer-material.html
        Material mat = GetComponent<Renderer>().material;
        Debug.Log("Finding material: " + mat.name + " and shader: " + mat.shader);
        Debug.Log("Setting exist equal to true!");
        exist = true;

        panel.GetComponent<ImageAnimation>().build(pathExtract);

    }
}

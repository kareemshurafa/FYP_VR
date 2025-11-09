using UnityEngine;
using System.IO;
using System.Collections.Generic;
using Dummiesman;

public class VentricleAnimation : MonoBehaviour
{
    // Serialized list to hold the meshes
    [SerializeField] List<Mesh> ventricleMesh = new List<Mesh>();

    // Serialized mesh renderer
    [SerializeField] MeshRenderer meshRenderer;

    // Serialized path for locating folder in Resources with meshes
    [SerializeField] string path;

    // Counter for mesh loop
    int counter;

    bool exist = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Reference - ChatGPT-4o (OpenAI)
        // Locates .obj files in Resources folder based on path, and creates Object array loaded with GameObject types
        Object[] files = Resources.LoadAll(path, typeof(GameObject));

        // Loops through each .obj file to extract the mesh, process it in Unity, and render it at runtime
        foreach (Object file in files)
        {
            Debug.Log("" + file.ToString());

            // Reference - ChatGPT-4o (OpenAI)
            // Create new child game object
            GameObject x = GameObject.Instantiate(file as GameObject, transform);

            // Obtain the current mesh and add to parent object's mesh list
            Mesh mesh1 = x.GetComponentInChildren<MeshFilter>().mesh;
            ventricleMesh.Add(mesh1);

            // Destroying the game object
            Destroy(x);
        }
        
        // Add a MeshFilter component to display the meshes in the list
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        
        // Add a MeshRenderer component and set to red
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material.color = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        while (!exist)
        {
            meshAnimation();
        }
    }

    // Looping through the mesh list to simulate an animation
    void meshAnimation()
    {
        GetComponent<MeshFilter>().mesh = ventricleMesh[counter];
        counter++;
        if (counter == ventricleMesh.Count)
        {
            counter = 0;
        }
    }

    public void build()
    {
        // adding implementation later
    }
}

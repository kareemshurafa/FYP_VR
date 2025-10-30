using UnityEngine;
using System.IO;
using System.Collections.Generic;
using Oculus.Interaction;
using Siccity.GLTFUtility;
using Unity.SharpZipLib.Utils;

public class Glbloader : MonoBehaviour
{
    // Serialized list to hold the meshes
    [SerializeField] List<Mesh> ventricleMesh = new List<Mesh>();

    // Serialized mesh renderer
    [SerializeField] MeshRenderer meshRenderer;

    // Serialized path for locating folder in Resources with meshes
    [SerializeField] string path;

    // Counter for mesh loop
    int counter;

    public string glbFilePath;

    private Mesh[] meshes;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Reference - https://github.com/Siccity/GLTFUtility
        GameObject glbObject = Importer.LoadFromFile(glbFilePath);

        MeshFilter[] meshFilters = glbObject.GetComponentsInChildren<MeshFilter>();

        meshes = new Mesh[meshFilters.Length];

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();

        for (int i = 0; i < meshFilters.Length; i++)
            {
                meshes[i] = meshFilters[i].mesh;
            }

        Destroy(glbObject); // optional cleanup
        
        // Add a MeshRenderer component and set to red
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material.color = Color.red;

        // Adding required components to create an interactable object in VR
        Rigidbody rigid = gameObject.AddComponent<Rigidbody>();
        rigid.useGravity = false;
        SphereCollider sphere = gameObject.AddComponent<SphereCollider>();

        // Obtained from Oculus.Interaction library
        Grabbable grabbable = gameObject.AddComponent<Grabbable>();

        GameObject interactor = new GameObject("Interactor");
        interactor.transform.parent = transform;

        // Reference - ChatGPT-4o (OpenAI)
        // var rayGrabbable = interactor.AddComponent<RayInteractable>();
        // rayGrabbable.InjectSurface(null);
    }

    // Update is called once per frame
    void Update()
    {
        meshAnimation();
    }

    // Looping through the mesh list to simulate an animation
    void meshAnimation()
    {
        GetComponent<MeshFilter>().mesh = meshes[counter];
        counter++;
        if (counter == meshes.Length)
        {
            counter = 0;
        }
    }
}

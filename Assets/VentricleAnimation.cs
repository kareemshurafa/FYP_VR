using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class VentricleAnimation : MonoBehaviour
{
    [SerializeField] List<Mesh> ventricleMesh = new List<Mesh>();

    [SerializeField] MeshRenderer meshRenderer;

    int counter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Reference - ChatGPT-4o (OpenAI)
        Object[] files = Resources.LoadAll("ResourcesMeshes/meshes", typeof(GameObject));
        //
        
        List<GameObject> games = new List<GameObject>();

        foreach (Object file in files)
        {
            Debug.Log("" + file.ToString());
            // Reference - ChatGPT-4o (OpenAI)
            GameObject x = GameObject.Instantiate(file as GameObject, transform);
            //
            games.Add(x);
            Mesh mesh1 = x.GetComponentInChildren<MeshFilter>().mesh;
            ventricleMesh.Add(mesh1);
            Destroy(x);
        }
    }

    // Update is called once per frame
    void Update()
    {
        animation();
    }

    void animation()
    {
        GetComponent<MeshFilter>().mesh = ventricleMesh[counter];
        counter++;
        if (counter == ventricleMesh.Count)
        {
            counter = 0;
        }
    }
}

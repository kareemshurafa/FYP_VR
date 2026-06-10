using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using System;

public class VectorCalculatorMasks : MonoBehaviour
{
    public static int playCounter = 0;

    public static int count = 93;

    public static float timer = 0.0f;

    public static string[] affinesList;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Build()
    {
        affinesList = VentricleAnimation.affinesList;
        List<string> lines = new List<string>();
        
        foreach (string affine in affinesList)
        {
            Debug.Log("affine here " + affine);
            // Reference - adapted from https://learn.microsoft.com/en-us/troubleshoot/developer/visualstudio/csharp/language-compilers/read-write-text-file
            StreamReader sr = new StreamReader(affine);
            Debug.Log("Created new StreamReader!");
            string line = "";

            line = sr.ReadLine();
            
            while (line != null)
            {
                lines.Add(line);
                line = sr.ReadLine();

            }
        }
        
        List<List<string[]>> comboLines = new List<List<string[]>>();
        foreach (string affine in affinesList)
        {
            List<string[]> comboEntry = new List<string[]>();
            for (int i = 0; i < 4 ; i++)
            {
                // Reference - https://learn.microsoft.com/en-us/dotnet/api/system.string.split?view=net-10.0
                comboEntry.Add(lines[0].Split(' '));
                lines.RemoveAt(0);
            }
            comboLines.Add(comboEntry);
        }
        Debug.Log("Number of combolines" + comboLines.Count);
        
        List<Vector4> vectors = new List<Vector4>();
        
        for (int counter = 0; counter < comboLines.Count ; counter++)
        {
            for (int i = 0; i < 4; i++)
            {
                Debug.Log("Starting vector run number " + i);
                List<float> floats = new List<float>();
                // Reference - https://learn.microsoft.com/en-us/dotnet/api/system.single.parse?view=net-10.0
                floats.Add(Single.Parse(comboLines[counter][0][i]));
                floats.Add(Single.Parse(comboLines[counter][1][i]));
                floats.Add(Single.Parse(comboLines[counter][2][i]));
                floats.Add(Single.Parse(comboLines[counter][3][i]));
                vectors.Add(new Vector4(floats[0], floats[1], floats[2], floats[3]));
            }
        }

        
        Debug.Log("Vectors: " + vectors.Count);
        foreach (Vector4 vector in vectors)
        {
            Debug.Log(vector);
        }

        // Reference - https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Matrix4x4.html
        // first 3 columns are x, y, z axes, and last column contains position information

        List<Matrix4x4> matrixList = new List<Matrix4x4> ();
        int index = 0;
        for (int i = 0 ; i < comboLines.Count ; i++)
        {
            Matrix4x4 matrix = new Matrix4x4(vectors[index], vectors[index + 1], vectors[index + 2], vectors[index + 3]);
            matrixList.Add(matrix);
            index += 4;
        }

        if (matrixList.Count > 0)
        {
            for (int i = 0; i < matrixList.Count; i++) {
                Matrix4x4 matrix = matrixList[i];
                
                Vector3 imagePosition = matrix.GetColumn(3)/1000f;

                // Reference - https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Quaternion.html
                // extract position and rotation from the matrix
                Quaternion rotation = matrix.rotation;
                
                GameObject empty = transform.GetChild(i).gameObject;
                
                // Reference - https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Transform-localPosition.html
                empty.transform.rotation = rotation;
                empty.transform.localPosition = imagePosition;

                GameObject child = transform.GetChild(i).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
                // Reference - https://docs.unity3d.com/6000.3/Documentation/ScriptReference/CanvasGroup.html
                CanvasGroup cg = child.GetComponent<CanvasGroup>();
                cg.alpha = 0.25f;

                Debug.Log("finished for image!" + i);
            }
        }
    }
}
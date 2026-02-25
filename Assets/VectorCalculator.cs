using UnityEngine;
using System.Collections.Generic;

public class VectorCalculator : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Reference - https://docs.unity3d.com/Packages/com.unity.ugui@2.0/manual/class-CanvasGroup.html
        // making semi transparent

        // list to hold vectors
        List<List<float>> vectors_first = new List<List<float>> { new List<float> {-73.5827f, 35.3296f, 125.2187f} , new List<float> {-80.3740f, 6.1151f, 124.7174f}, 
        new List<float> {-142.7890f, 36.7240f, 27.2139f}, new List<float> {-122.2727f, 89.2387f, -37.3604f}, new List<float> {26.7434f, 121.0056f, -117.8789f}};
        List<List<float>> vectors_second = new List<List<float>> { new List<float> {-99.7267f,  72.7450f,  16.1987f}, new List<float> {-101.4347f,   61.4907f,   22.4493f}, 
        new List<float> {-109.8808f,  -52.6082f,  -42.8261f}, new List<float> {-93.5513f, -10.0629f, -94.6584f}, new List<float> {  28.0472f,    6.5079f, -147.1596f} };
        List<List<float>> vectors_third = new List<List<float>> { new List<float> {-61.2490f,  46.6338f,  -2.1288f}, new List<float> {-62.2933f,  38.3414f,   1.7058f}, 
        new List<float> {-61.8589f, -40.8828f, -35.4343f}, new List<float> {-52.8998f, -14.9943f, -65.9991f}, new List<float> {19.2864f,  -5.9015f, -99.5416f} };

        for (int i = 0; i < 5; i++) {
            
            // Reference - https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Vector3.html
            // obtaining 3 vectors
            Vector3 vecA = new Vector3(vectors_first[i][0], vectors_first[i][1], vectors_first[i][2]);
            Vector3 vecB = new Vector3(vectors_second[i][0], vectors_second[i][1], vectors_second[i][2]);
            Vector3 vecC = new Vector3(vectors_third[i][0], vectors_third[i][1], vectors_third[i][2]);

            // finding vector differences
            Vector3 vecD = vecB - vecA;
            Vector3 vecE = vecC - vecA;

            // crossing vectors and normalising to find normal vector
            Vector3 normal = Vector3.Cross(vecD, vecE).normalized;

            // Reference - https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Quaternion.LookRotation.html
            Quaternion rotation = Quaternion.LookRotation(normal);

            GameObject child = transform.GetChild(i).gameObject;
            child.transform.rotation = rotation;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
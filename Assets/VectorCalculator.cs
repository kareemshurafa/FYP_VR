using UnityEngine;
using System.Collections.Generic;

public class VectorCalculator : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // // Reference - https://docs.unity3d.com/Packages/com.unity.ugui@2.0/manual/class-CanvasGroup.html
        // // making semi transparent

        // // list to hold vectors
        // List<List<float>> vectors_first = new List<List<float>> { new List<float> {-73.5827f, 35.3296f, 125.2187f} , new List<float> {-80.3740f, 6.1151f, 124.7174f}, 
        // new List<float> {-142.7890f, 36.7240f, 27.2139f}, new List<float> {-122.2727f, 89.2387f, -37.3604f}, new List<float> {26.7434f, 121.0056f, -117.8789f}};
        // List<List<float>> vectors_second = new List<List<float>> { new List<float> {-99.7267f,  72.7450f,  16.1987f}, new List<float> {-101.4347f,   61.4907f,   22.4493f}, 
        // new List<float> {-109.8808f,  -52.6082f,  -42.8261f}, new List<float> {-93.5513f, -10.0629f, -94.6584f}, new List<float> {  28.0472f,    6.5079f, -147.1596f} };
        // List<List<float>> vectors_third = new List<List<float>> { new List<float> {-61.2490f,  46.6338f,  -2.1288f}, new List<float> {-62.2933f,  38.3414f,   1.7058f}, 
        // new List<float> {-61.8589f, -40.8828f, -35.4343f}, new List<float> {-52.8998f, -14.9943f, -65.9991f}, new List<float> {19.2864f,  -5.9015f, -99.5416f} };

        // for (int i = 0; i < 5; i++) {
            
        //     // Reference - https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Vector3.html
        //     // obtaining 3 vectors
        //     Vector3 vecA = new Vector3(vectors_first[i][0], vectors_first[i][1], vectors_first[i][2]);
        //     Vector3 vecB = new Vector3(vectors_second[i][0], vectors_second[i][1], vectors_second[i][2]);
        //     Vector3 vecC = new Vector3(vectors_third[i][0], vectors_third[i][1], vectors_third[i][2]);

        //     // finding vector differences
        //     Vector3 vecD = vecB - vecA;
        //     Vector3 vecE = vecC - vecA;

        //     // crossing vectors and normalising to find normal vector
        //     Vector3 normal = Vector3.Cross(vecE, vecD).normalized;

        //     // Reference - ChatGPT-5
        //     // we obtain the edge vector (vecEdge) defined as a chosen edge direction
        //     Vector3 vecEdge = vecD.normalized;
        //     // we then obtain the perpendicular vector of the normal and vecEdge to define a new consistent axis
        //     Vector3 vecPerpendicular = Vector3.Cross(normal, vecEdge).normalized;
        //     // Reference - https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Quaternion.LookRotation.html
        //     Quaternion rotation = Quaternion.LookRotation(normal, vecPerpendicular);

        //     GameObject child = transform.GetChild(i).gameObject;
        //     child.transform.rotation = rotation;
        // }


        // Matrix for scan view 1:
        // 93.162712 -20.256348 0.000000 31.170082
        // -63.220787 28.854599 0.000000 -76.524963
        // -44.374756 -83.636475 1.000000 283.098083
        // 0.000000 0.000000 0.000000 1.000000

        
        // Reference - https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Matrix4x4.html
        // first 3 columns are x, y, z axes, and last column contains position information
        //Vector3 xAxis = 
        Matrix4x4 matrix1 = new Matrix4x4(new Vector4 (93.162712f, -63.220787f,-44.374756f,0.000000f), new Vector4 (-20.256348f, 28.854599f,-83.636475f,0.000000f), 
                                        new Vector4 (0.000000f, 0.000000f,1.000000f,0.000000f), new Vector4 (31.170082f, -76.524963f,283.098083f,1.000000f));

        Matrix4x4 matrix2 = new Matrix4x4(new Vector4 (94.769508f, -56.049412f,-50.224304f,0.000000f), new Vector4 (-16.355728f, 42.633415f,-78.440125f,0.000000f), 
                                        new Vector4 (0.000000f, 0.000000f,1.000000f,0.000000f), new Vector4 (29.756002f, -85.249390f,281.770081f,1.000000f));

        Matrix4x4 matrix3 = new Matrix4x4(new Vector4 (116.271240f, 28.389664f,17.897095f,0.000000f), new Vector4 (25.048309f, -68.657104f,-53.821472f,0.000000f), 
                                        new Vector4 (0.000000f, 0.000000f,1.000000f,0.000000f), new Vector4 (28.864300f, -77.775986f,276.184601f,1.000000f));

        Matrix4x4 matrix4 = new Matrix4x4(new Vector4 (98.426071f, -11.939997f,69.390381f,0.000000f), new Vector4 (21.867346f, -76.235542f,-44.135353f,0.000000f), 
                                        new Vector4 (-0.067409f, 0.225787f,0.971842f,0.000000f), new Vector4 (28.460659f, -72.916397f,272.466370f,1.000000f));

        Matrix4x4 matrix5 = new Matrix4x4(new Vector4 (-21.211817f, -30.045977f,115.293404f,0.000000f), new Vector4 (1.041977f, -87.870613f,-22.707745f,0.000000f), 
                                        new Vector4 (0.186465f, 0.652626f, 0.734377f,0.000000f), new Vector4 (37.708900f,  -70.501999f,258.563080f,1.000000f));

        List<Matrix4x4> matrixList = new List<Matrix4x4> ();
        matrixList.Add(matrix1);
        matrixList.Add(matrix2);
        matrixList.Add(matrix3);
        matrixList.Add(matrix4);
        matrixList.Add(matrix5);

        // first affine position to subtract from all positions
        // need to divide by 1000 to convert from mm scale to m scale in Unity
        Vector3 firstPosition = matrixList[0].GetColumn(3)/1000;
        Debug.Log("first position = " + firstPosition);
        
        for (int i = 0; i < 5; i++) {
            Matrix4x4 matrix = matrixList[i];
            Vector3 imagePosition = matrix.GetColumn(3)/1000;
            Debug.Log("image position = " + imagePosition);

            // Reference - adapted from ChatGPT-5.0
            // need to flip the z-axis inside the 4x4 matrix to account for difference in coordinate definitions
            // Reference - https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Matrix4x4.Scale.html
            Vector3 zFlipVector = new Vector3 (1, 1, -1);
            Matrix4x4 zFlipMatrix = Matrix4x4.Scale(zFlipVector);
            Debug.Log("zFlipMatrix " + zFlipMatrix);
            // need to convert the coordinate system by flipping z-axis
            matrix = zFlipMatrix * matrix * zFlipMatrix;

            // extract position and rotation from the matrix
            Quaternion rotation = matrix.rotation;
            
            GameObject child = transform.GetChild(i).gameObject;
            child.transform.rotation = rotation;
            // Reference - https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Transform-localPosition.html
            child.transform.localPosition = imagePosition - firstPosition;
        }
        

        







    
    

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
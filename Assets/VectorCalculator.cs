using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class VectorCalculator : MonoBehaviour
{
    
    [SerializeField] List<Texture2D> imageTexturesList1 = new List<Texture2D>();
    [SerializeField] List<Texture2D> imageTexturesList2 = new List<Texture2D>();
    [SerializeField] List<Texture2D> imageTexturesList3 = new List<Texture2D>();
    [SerializeField] List<Texture2D> imageTexturesList4 = new List<Texture2D>();
    [SerializeField] List<Texture2D> imageTexturesList5 = new List<Texture2D>();

    public static int playCounter = 0;

    public static int count = 93;

    public static float timer = 0.0f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Reference - https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Matrix4x4.html
        // first 3 columns are x, y, z axes, and last column contains position information
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

        Debug.Log("column 1 magnitude: " + matrixList[0].GetColumn(0).magnitude);
        Debug.Log("column 2 magnitude: " + matrixList[0].GetColumn(1).magnitude);
        Debug.Log("column 3 magnitude: " + matrixList[0].GetColumn(2).magnitude);
        
        // first affine position to subtract from all positions
        // need to divide by 1000 to convert from mm scale to m scale in Unity
        Vector3 firstPosition = matrixList[0].GetColumn(3)/1000f;

        // float halfX = matrixList[0].GetColumn(0).magnitude / 1000f / 2f;
        // float halfY = matrixList[0].GetColumn(1).magnitude / 1000f / 2f;
        // Vector3 half = new Vector3(halfX, -halfY, 0);
        // firstPosition += half; 
        // firstPosition.z = -firstPosition.z;

        Debug.Log("first position = " + firstPosition);
        
        for (int i = 0; i < 5; i++) {
            Matrix4x4 matrix = matrixList[i];
            
            Vector3 imagePosition = matrix.GetColumn(3)/1000f;
            float halfXX = matrix.GetColumn(0).magnitude / 1000f / 2f;
            float halfYY = matrix.GetColumn(1).magnitude / 1000f / 2f;
            
            Vector3 halff = new Vector3(-halfXX, -halfYY, 0);
            // imagePosition += halff; 
            
            imagePosition.z = -imagePosition.z;
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
            
            GameObject empty = transform.GetChild(i).gameObject;
            GameObject quad = transform.GetChild(i).GetChild(0).gameObject;
            
            empty.transform.rotation = rotation;
            empty.transform.localPosition = imagePosition - firstPosition;
            // Reference - https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Transform-localPosition.html
            quad.transform.localPosition = halff;

            Debug.Log("finished for image!" + i);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += 0.05f;
        // Debug.Log("Timer not done");
        if (timer >= 1.0f)
        {
            // Reference - https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Component.GetComponentInChildren.html
            GameObject child1 = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
            GameObject child2 = transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
            GameObject child3 = transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
            GameObject child4 = transform.GetChild(3).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
            GameObject child5 = transform.GetChild(4).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
            
            RawImage raws1 = child1.GetComponent<RawImage>();
            RawImage raws2 = child2.GetComponent<RawImage>();
            RawImage raws3 = child3.GetComponent<RawImage>();
            RawImage raws4 = child4.GetComponent<RawImage>();
            RawImage raws5 = child5.GetComponent<RawImage>();

            raws1.texture = imageTexturesList1[playCounter];
            raws2.texture = imageTexturesList2[playCounter];
            raws3.texture = imageTexturesList3[playCounter];
            raws4.texture = imageTexturesList4[playCounter];
            raws5.texture = imageTexturesList5[playCounter];
            playCounter++;
            // Debug.Log("Incremented!");
            timer = 0.0f;
            if (playCounter >= count) 
            {
                playCounter = 0;
            }
        }
    }
}
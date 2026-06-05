using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class PNGScans : MonoBehaviour
{
    public static List<List<Texture2D>> imageTexturesList;
    
    public GameObject affineParent;
    
    // public static bool play;
    // public static int playCounter;
    // public static int count;
    // public static float timer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void buildImage()
    {
        // GameObject child1 = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
        // RawImage raws1 = child1.GetComponent<RawImage>();
        // GameObject child2 = transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).gameObject;
        // RawImage raws2 = child2.GetComponent<RawImage>();
        // GameObject child3 = transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).gameObject;
        // RawImage raws3 = child3.GetComponent<RawImage>();
        // GameObject child4 = transform.GetChild(3).GetChild(0).GetChild(0).GetChild(0).gameObject;
        // RawImage raws4 = child4.GetComponent<RawImage>();
        // GameObject child5 = transform.GetChild(4).GetChild(0).GetChild(0).GetChild(0).gameObject;
        // RawImage raws5 = child5.GetComponent<RawImage>();
        // GameObject child6 = transform.GetChild(5).GetChild(0).GetChild(0).GetChild(0).gameObject;
        // RawImage raws6 = child6.GetComponent<RawImage>();
        // raws1.texture = imageTexturesList[0][0];
        // raws2.texture = imageTexturesList[1][0];
        // raws3.texture = imageTexturesList[2][0];
        // raws4.texture = imageTexturesList[3][0];
        // raws5.texture = imageTexturesList[4][9];
        // raws6.texture = imageTexturesList[5][0];
        imageTexturesList = ImageAnimation.imageTexturesList;
        Debug.Log("Image Textures List " + imageTexturesList.Count);
        
        for (int i = 0; i < imageTexturesList.Count; i++) // temporarily using third.count
        {
            GameObject child = transform.GetChild(i).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
            RawImage raws = child.GetComponent<RawImage>();
            raws.texture = imageTexturesList[i][0];
        }
        affineParent.GetComponent<VectorCalculator>().build();
    }

    public void incrementer(int playCounter, int count)
    {
        for (int i = 0; i < count ; i++)
        {
            GameObject child = transform.GetChild(i).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
            RawImage raws = child.GetComponent<RawImage>();
            raws.texture = imageTexturesList[i][playCounter];
        }
    }
}

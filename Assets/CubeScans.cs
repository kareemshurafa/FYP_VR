using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class CubeScans : MonoBehaviour
{
    public static List<List<Texture2D>> imageTexturesList;
    // public static bool play;
    // public static int playCounter;
    // public static int count;
    // public static float timer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        imageTexturesList = ImageAnimation.imageTexturesList;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void incrementer(int playCounter)
    {
        GameObject child1 = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
        RawImage raws1 = child1.GetComponent<RawImage>();
        GameObject child2 = transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).gameObject;
        RawImage raws2 = child2.GetComponent<RawImage>();
        GameObject child3 = transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).gameObject;
        RawImage raws3 = child3.GetComponent<RawImage>();
        GameObject child4 = transform.GetChild(3).GetChild(0).GetChild(0).GetChild(0).gameObject;
        RawImage raws4 = child4.GetComponent<RawImage>();
        GameObject child5 = transform.GetChild(4).GetChild(0).GetChild(0).GetChild(0).gameObject;
        RawImage raws5 = child5.GetComponent<RawImage>();
        GameObject child6 = transform.GetChild(5).GetChild(0).GetChild(0).GetChild(0).gameObject;
        RawImage raws6 = child6.GetComponent<RawImage>();
        raws1.texture = imageTexturesList[0][playCounter];
        raws2.texture = imageTexturesList[1][playCounter];
        raws3.texture = imageTexturesList[2][playCounter];
        raws4.texture = imageTexturesList[3][playCounter];
        raws5.texture = imageTexturesList[4][playCounter];
        raws6.texture = imageTexturesList[5][playCounter];
    }
}

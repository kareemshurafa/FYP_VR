using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class CubeScans : MonoBehaviour
{
    
    public static List<Texture2D> image1Textures;
    public static int playCounter = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // accessing static member (for now)
        image1Textures = ImageAnimation.image1Textures;
        Debug.Log("length of images  === " + image1Textures.Count);
    }

    // Update is called once per frame
    void Update()
    {
        // Reference -  https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Transform.GetChild.html
        GameObject child = transform.GetChild(0).GetChild(0).gameObject;
        RawImage raws = child.GetComponent<RawImage>();
        raws.texture = image1Textures[playCounter];
        playCounter++;
        if (playCounter >= image1Textures.Count) // temporarily using image3
        {
            playCounter = 0;
        }
    }
}

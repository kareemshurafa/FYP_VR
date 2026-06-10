using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PNGScans : MonoBehaviour
{
    public static List<List<Texture2D>> imageTexturesList;
    
    public GameObject affineParent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BuildImage()
    {
        imageTexturesList = ImageAnimation.imageTexturesList;
        Debug.Log("Image Textures List " + imageTexturesList.Count);
        
        for (int i = 0; i < imageTexturesList.Count; i++)
        {
            // get the child game object to change the image
            GameObject child = transform.GetChild(i).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
            RawImage raws = child.GetComponent<RawImage>();
            raws.texture = imageTexturesList[i][0];
        }

        int affineViews = VentricleAnimation.affinesList.Length;
        // currently takes up to 6 different scan views
        int unusedViews = 6 - affineViews;
        if (unusedViews > 0)
        {
            for (int i = 0 ; i < unusedViews ; i++)
            {
            GameObject child = transform.GetChild(5 - i).gameObject;
            Destroy(child);
            }
        }
        // calling the vector calculator script for affines
        affineParent.GetComponent<VectorCalculator>().Build();            
    }

    public void Incrementer(int playCounter, int count)
    {
        for (int i = 0; i < count ; i++)
        {
            GameObject child = transform.GetChild(i).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
            RawImage raws = child.GetComponent<RawImage>();
            raws.texture = imageTexturesList[i][playCounter];
        }
    }
}

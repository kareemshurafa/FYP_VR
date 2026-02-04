using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using System.Threading;

public class ImageAnimation : MonoBehaviour
{
    // public static List<Texture2D> image1Textures = new List<Texture2D>();
    // public static List<Texture2D> image2Textures = new List<Texture2D>();
    // public static List<Texture2D> image3Textures = new List<Texture2D>();
    // public static List<Texture2D> image4Textures = new List<Texture2D>();
    // public static List<Texture2D> image5Textures = new List<Texture2D>();
    // public static List<Texture2D> image6Textures = new List<Texture2D>();
    public static List<List<Texture2D>> imageTexturesList = new List<List<Texture2D>>();
    public static bool play;
    public static int playCounter = 0;
    public static int count = 0;
    public static float timer = 0.0f;
    [SerializeField] public GameObject scanParent;
    [SerializeField] public GameObject ventricle;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        play = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (play)
        {
            timer += 0.25f;
            incrementer();
        }
    }

    public void build(string pathExtract)
    {
        // add lists to list of lists imageTextures
        imageTexturesList.Add(new List<Texture2D>());
        imageTexturesList.Add(new List<Texture2D>());
        imageTexturesList.Add(new List<Texture2D>());
        imageTexturesList.Add(new List<Texture2D>());
        imageTexturesList.Add(new List<Texture2D>());
        imageTexturesList.Add(new List<Texture2D>());
        // first view
        buildImage(pathExtract, 1, 0);
        // second view
        buildImage(pathExtract, 2, 1);
        // third view
        buildImage(pathExtract, 3, 2);
        // fourth view
        buildImage(pathExtract, 5, 3);
        // fifth view
        buildImage(pathExtract, 6, 4);
        // sixth view
        buildImage(pathExtract, 7, 5);
        for (int i = 0; i < imageTexturesList.Count; i++)
        {
            int minCount = imageTexturesList[i].Count;
            if (count == 0 || minCount < count)
            {
                count = imageTexturesList[i].Count;
            }
        }
        Debug.Log("Min count: " + count);
        scanParent.GetComponent<CubeScans>().buildImage();
    }

    public void buildImage(string pathExtract, int pngIndex, int index)
    {
        // Reference - https://learn.microsoft.com/en-us/dotnet/api/system.io.directory?view=net-9.0
        string[] pngPaths = Directory.GetFiles(pathExtract, "*.png");
        List<string> pngPathList = new List<string>();
        
        // Reference - https://learn.microsoft.com/en-us/dotnet/api/system.string.contains?view=net-10.0
        foreach (string png in pngPaths)
        {
            if (png.Contains("--" + pngIndex + "-"))
            {
                pngPathList.Add(png);
            }
        }
        
        for (int i = 0; i < pngPathList.Count; i++) // temporarily using third.count
        {
            // Reference - https://docs.unity3d.com/6000.2/Documentation/ScriptReference/Windows.File.ReadAllBytes.html
            byte[] byteImage1 = File.ReadAllBytes(pngPathList[i]);
            // Reference - https://docs.unity3d.com/6000.0/Documentation/ScriptReference/ImageConversion.LoadImage.html
            // the textures will get overwritten, so creating an arbitrary file for now
            // Reference - https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Texture2D-ctor.html
            // using Alpha8 
            Texture2D texImage1 = new Texture2D(2, 2, TextureFormat.Alpha8, false);
            ImageConversion.LoadImage(texImage1, byteImage1);
            imageTexturesList[index].Add(texImage1);
        }
        
        Debug.Log("Length of current imageTextures  + " + index +  "List = " + imageTexturesList[index].Count);
        
        // Reference - https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Transform.GetChild.html
        GameObject child = transform.GetChild(index).gameObject;
        // Reference - https://docs.unity3d.com/Packages/com.unity.ugui@2.0/manual/script-RawImage.html
        RawImage raw = child.GetComponent<RawImage>();
        raw.texture = imageTexturesList[index][0];
    }

    public void changePlay()
    {
        play = true;
        Debug.Log("Changed to " + play);
    }

        public void changePause()
    {
        play = false;
        Debug.Log("Changed to " + play);
    }

    public void incrementer()
    {
        if (timer >= 1.0f)
        {
            GameObject child1 = transform.GetChild(0).gameObject;
            RawImage raws1 = child1.GetComponent<RawImage>();
            GameObject child2 = transform.GetChild(1).gameObject;
            RawImage raws2 = child2.GetComponent<RawImage>();
            GameObject child3 = transform.GetChild(2).gameObject;
            RawImage raws3 = child3.GetComponent<RawImage>();
            GameObject child4 = transform.GetChild(3).gameObject;
            RawImage raws4 = child4.GetComponent<RawImage>();
            GameObject child5 = transform.GetChild(4).gameObject;
            RawImage raws5 = child5.GetComponent<RawImage>();
            GameObject child6 = transform.GetChild(5).gameObject;
            RawImage raws6 = child6.GetComponent<RawImage>();
            raws1.texture = imageTexturesList[0][playCounter];
            raws2.texture = imageTexturesList[1][playCounter];
            raws3.texture = imageTexturesList[2][playCounter];
            raws4.texture = imageTexturesList[3][playCounter];
            raws5.texture = imageTexturesList[4][playCounter];
            raws6.texture = imageTexturesList[5][playCounter];
            scanParent.GetComponent<CubeScans>().incrementer(playCounter);
            ventricle.GetComponent<VentricleAnimation>().incrementer(playCounter);


            playCounter++;
            timer = 0.0f;
            if (playCounter >= count) 
            {
                playCounter = 0;
            }
        }
    }   
}

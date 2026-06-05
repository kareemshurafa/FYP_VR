using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using TMPro;

public class ImageAnimation : MonoBehaviour
{
    // public static List<Texture2D> image1Textures = new List<Texture2D>();
    // public static List<Texture2D> image2Textures = new List<Texture2D>();
    // public static List<Texture2D> image3Textures = new List<Texture2D>();
    // public static List<Texture2D> image4Textures = new List<Texture2D>();
    // public static List<Texture2D> image5Textures = new List<Texture2D>();
    // public static List<Texture2D> image6Textures = new List<Texture2D>();
    public static List<List<Texture2D>> imageTexturesList = new List<List<Texture2D>>();

    public static List<List<Texture2D>> predictionTexturesList = new List<List<Texture2D>>();
    public static bool play;
    public static int playCounter = 0;
    public static int count = 0;
    public static float timer = 0.0f;
    public static float timerIncrementer;
    [SerializeField] public GameObject scanParent;
    [SerializeField] public GameObject scanParent2;
    [SerializeField] public GameObject ventricle;
    [SerializeField] public Slider speedSlider;
    [SerializeField] public TMP_Text speedText;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        play = false;
        timerIncrementer = 0.5f;
        speedText.text = "Speed: " + timerIncrementer;
    }

    // Update is called once per frame
    void Update()
    {
        if (play)
        {
            timer += timerIncrementer;
            incrementer();
        }
    }

    public void speedSliderChange()
    {
        // Reference - adapted from: https://docs.unity3d.com/540/Documentation/ScriptReference/UI.Slider-onValueChanged.html (legacy documentation)
        timerIncrementer = (float) speedSlider.value/10;
        speedText.text = "Speed: " + (float) speedSlider.value/10;
        Debug.Log("changing speed slider valuer to: " + speedSlider.value + " with incrementer value of :" + timerIncrementer);
    }



    public void build(string[] pngsList, int option)
    {
        // Reference - https://learn.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex?view=netframework-4.8.1
        // Extracting number indexes for pngs and predictions
        Regex regex = new Regex("--[1-9]");
        List<string> indexes = new List<string>();
        
        foreach (string png in pngsList)
        {
            string split = regex.Match(png).ToString();
            Debug.Log("Match found: " + split);
            if (!indexes.Contains(split[2].ToString())) {
                indexes.Add(split[2].ToString());
            }
        }
        Debug.Log("Indexes found length: " + indexes.Count);
        
        if (option == 1)
        {
            // add lists to list of lists imageTextures
            for (int i = 0; i < indexes.Count ; i++)
            {
                imageTexturesList.Add(new List<Texture2D>());
            }

            for (int i = 0; i < indexes.Count ; i++)
            {
                // build the image lists for each of the different views
                buildImage(pngsList, indexes[i], i, option);
            }

            // // first view
            // buildImage(pngsList, 1, 0, option);
            // // second view
            // buildImage(pngsList, 2, 1, option);
            // // third view
            // buildImage(pngsList, 3, 2, option);
            // // fourth view
            // buildImage(pngsList, 5, 3, option);
            // // fifth view
            // buildImage(pngsList, 6, 4, option);
            // // sixth view
            // buildImage(pngsList, 7, 5, option);
            
            // finding the smallest number of frames to fix all the frames together
            for (int i = 0; i < imageTexturesList.Count; i++)
            {
                int minCount = imageTexturesList[i].Count;
                if (count == 0 || minCount < count)
                {
                    count = imageTexturesList[i].Count;
                }
            }
            Debug.Log("Min count: " + count);
            scanParent.GetComponent<PNGScans>().buildImage();
        }
        
        else if (option == 2)
        {
            // add lists to list of lists predictionTextures
            for (int i = 0; i < indexes.Count ; i++)
            {
                predictionTexturesList.Add(new List<Texture2D>());
            }

            for (int i = 0; i < indexes.Count ; i++)
            {
                // build the image lists for each of the different views
                buildImage(pngsList, indexes[i], i, option);
            }

            // // first view
            // buildImage(pngsList, 1, 0, option);
            // // second view
            // buildImage(pngsList, 2, 1, option);
            // // third view
            // buildImage(pngsList, 3, 2, option);
            // // fourth view
            // buildImage(pngsList, 5, 3, option);
            // // fifth view
            // buildImage(pngsList, 6, 4, option);
            // // sixth view
            // buildImage(pngsList, 7, 5, option);
            
            // finding the smallest number of frames to fix all the frames together
            for (int i = 0; i < predictionTexturesList.Count; i++)
            {
                int minCount = predictionTexturesList[i].Count;
                if (count == 0 || minCount < count)
                {
                    count = predictionTexturesList[i].Count;
                }
            }
            Debug.Log("Min count: " + count);
            scanParent2.GetComponent<PredictionScans>().buildImage();
        }

        
    }

    public void buildImage(string[] pngsList, string pngIndex, int index, int option)
    {
        
        if (option == 1)
        {
            // Reference - https://learn.microsoft.com/en-us/dotnet/api/system.io.directory?view=net-9.0
            // string[] pngPaths = Directory.GetFiles(pathExtract, "*.png");
            List<string> pngPathList = new List<string>();
            
            // Reference - https://learn.microsoft.com/en-us/dotnet/api/system.string.contains?view=net-10.0
            foreach (string png in pngsList)
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

        else if (option == 2)
        {
            // Reference - https://learn.microsoft.com/en-us/dotnet/api/system.io.directory?view=net-9.0
            // string[] pngPaths = Directory.GetFiles(pathExtract, "*.png");
            List<string> pngPathList = new List<string>();
            
            // Reference - https://learn.microsoft.com/en-us/dotnet/api/system.string.contains?view=net-10.0
            foreach (string png in pngsList)
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
                predictionTexturesList[index].Add(texImage1);
            }
            
            Debug.Log("Length of current predictionTextures  + " + index +  "List = " + predictionTexturesList[index].Count);
            
            // Reference - https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Transform.GetChild.html
            GameObject child = transform.GetChild(index).gameObject;
            // // Reference - https://docs.unity3d.com/Packages/com.unity.ugui@2.0/manual/script-RawImage.html
            // RawImage raw = child.GetComponent<RawImage>();
            // raw.texture = predictionTexturesList[index][0];  
        }
        

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
            
            for (int i = 0; i < imageTexturesList.Count ; i++)
            {
                GameObject child = transform.GetChild(i).gameObject;
                RawImage raws = child.GetComponent<RawImage>();
                raws.texture = imageTexturesList[i][playCounter];
            }
            
            scanParent.GetComponent<PNGScans>().incrementer(playCounter, imageTexturesList.Count);
            scanParent2.GetComponent<PredictionScans>().incrementer(playCounter, imageTexturesList.Count);
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

using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine.SceneManagement;

public class ImageAnimation : MonoBehaviour
{
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

    bool modelVisible = true;

    bool scansVisible = true;
    
    bool predictionsVisible = true;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // initially paused on load
        play = false;
        timerIncrementer = 0.5f;
        speedText.text = "Speed: " + timerIncrementer;
    }

    // Update is called once per frame
    void Update()
    {
        // checks if play button is pressed
        if (play)
        {
            // increments timer based on the speed selected
            // calls Incrementer() that sends out updates to all images and models
            timer += timerIncrementer;
            if (timer > 1.0)
            {
                Incrementer();
            }
        }
    }

    public void SpeedSliderChange()
    {
        // Reference - adapted from: https://docs.unity3d.com/540/Documentation/ScriptReference/UI.Slider-onValueChanged.html (legacy documentation)
        timerIncrementer = (float) speedSlider.value/10;
        speedText.text = "Speed: " + (float) speedSlider.value/10;
        Debug.Log("changing speed slider valuer to: " + speedSlider.value + " with incrementer value of :" + timerIncrementer);
    }

    public void Build(string[] pngsList, int option)
    {
        // Reference - https://learn.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex?view=netframework-4.8.1
        // extracting number indexes for pngs and predictions
        Regex regex = new Regex("--[1-9]");
        List<string> indexes = new List<string>();
        
        foreach (string png in pngsList)
        {
            // extracts the number of different views and their specific view numbers in the file name
            string split = regex.Match(png).ToString();
            Debug.Log("Match found: " + split);
            if (!indexes.Contains(split[2].ToString())) {
                indexes.Add(split[2].ToString());
            }
        }
        Debug.Log("Indexes found length: " + indexes.Count);
        
        // pngs
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
                BuildImage(pngsList, indexes[i], i, option);
            }
            
            // finding the smallest number of frames to fix all the frames together
            for (int i = 0; i < imageTexturesList.Count; i++)
            {
                int minCount = imageTexturesList[i].Count;
                if (count == 0 || minCount < count)
                {
                    count = imageTexturesList[i].Count;
                }
            }
            int viewNumbers = imageTexturesList.Count;
            int unusedViews = 6 - viewNumbers;
            if (unusedViews > 0)
            {
                for (int i = 0 ; i < unusedViews ; i++)
                {
                GameObject child = transform.GetChild(5 - i).gameObject;
                Destroy(child);
                }
            }

            Debug.Log("Min count: " + count);
            scanParent.GetComponent<PNGScans>().BuildImage();
        }
        
        // predictions
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
                BuildImage(pngsList, indexes[i], i, option);
            }

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
            scanParent2.GetComponent<PredictionScans>().BuildImage();
        }

        
    }

    public void BuildImage(string[] pngsList, string pngIndex, int index, int option)
    {
        // Reference - https://learn.microsoft.com/en-us/dotnet/api/system.io.directory?view=net-9.0
        // string[] pngPaths = Directory.GetFiles(pathExtract, "*.png");
        List<string> pngPathList = new List<string>();
        
        // Reference - https://learn.microsoft.com/en-us/dotnet/api/system.string.contains?view=net-10.0
        // adding the images of the current view
        foreach (string png in pngsList)
        {
            if (png.Contains("--" + pngIndex + "-"))
            {
                pngPathList.Add(png);
            }
        }        
        if (option == 1)
        {
            for (int i = 0; i < pngPathList.Count; i++)
            {
                // Reference - https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Windows.File.ReadAllBytes.html
                byte[] byteImage1 = File.ReadAllBytes(pngPathList[i]);
                // Reference - https://docs.unity3d.com/6000.3/Documentation/ScriptReference/ImageConversion.LoadImage.html
                // the textures will get overwritten, so creating an arbitrary file for now
                // Reference - https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Texture2D-ctor.html
                // using Alpha8 
                Texture2D texImage1 = new Texture2D(2, 2, TextureFormat.Alpha8, false);
                ImageConversion.LoadImage(texImage1, byteImage1);
                imageTexturesList[index].Add(texImage1);
            }
            Debug.Log("Length of current imageTextures  + " + index +  "List = " + imageTexturesList[index].Count);
            // Reference - https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Transform.GetChild.html
            // getting the current image panel to overlay the image onto
            GameObject child = transform.GetChild(index).gameObject;
            // Reference - https://docs.unity3d.com/Packages/com.unity.ugui@2.0/manual/script-RawImage.html
            RawImage raw = child.GetComponent<RawImage>();
            raw.texture = imageTexturesList[index][0];   
        }

        else if (option == 2)
        {
            for (int i = 0; i < pngPathList.Count; i++) // temporarily using third.count
            {
                // Reference - https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Windows.File.ReadAllBytes.html
                byte[] byteImage1 = File.ReadAllBytes(pngPathList[i]);
                // Reference - https://docs.unity3d.com/6000.3/Documentation/ScriptReference/ImageConversion.LoadImage.html
                // the textures will get overwritten, so creating an arbitrary file for now
                // Reference - https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Texture2D-ctor.html
                // using Alpha8 
                Texture2D texImage1 = new Texture2D(2, 2, TextureFormat.Alpha8, false);
                ImageConversion.LoadImage(texImage1, byteImage1);
                predictionTexturesList[index].Add(texImage1);
            }
            Debug.Log("Length of current predictionTextures  + " + index +  "List = " + predictionTexturesList[index].Count);
        }
    }

    public void ChangePlay()
    {
        play = true;
        Debug.Log("Changed to " + play);
    }

        public void ChangePause()
    {
        play = false;
        Debug.Log("Changed to " + play);
    }

    public void ResetScene()
    {
        // Reference - https://docs.unity3d.com/6000.3/Documentation/ScriptReference/SceneManagement.SceneManager.LoadScene.html
        SceneManager.LoadScene("FYPScene");
    }

    public void ToggleModelVisibility(GameObject gameobject)
    {
        MeshRenderer meshRenderer = gameobject.GetComponent<MeshRenderer>();
        modelVisible = !modelVisible;
        
        if (modelVisible)
        {
            // Reference - https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Color.html
            meshRenderer.material.color = new Color(1.0f, 0.0f, 0.0f, 0.5f);
        }
        else
        {
            meshRenderer.material.color = new Color(1.0f, 0.0f, 0.0f, 0.0f);
        }

    }

    public void ToggleScansVisibility(GameObject gameobject)
    {
        scansVisible = !scansVisible;
        if (scansVisible)
        {
            for (int i = 0; i < imageTexturesList.Count; i++)
            {
                Debug.Log("performing child get ");
                GameObject child = gameobject.transform.GetChild(i).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
                Debug.Log("performed child get ");
                // Reference - https://docs.unity3d.com/6000.3/Documentation/ScriptReference/CanvasGroup.html
                CanvasGroup cg = child.GetComponent<CanvasGroup>();
                cg.alpha = 0.25f;
            }
        }
        else
        {
            for (int i = 0; i < imageTexturesList.Count; i++)
            {
                Debug.Log("performing child get ");
                GameObject child = gameobject.transform.GetChild(i).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
                Debug.Log("performed child get ");
                // Reference - https://docs.unity3d.com/6000.3/Documentation/ScriptReference/CanvasGroup.html
                CanvasGroup cg = child.GetComponent<CanvasGroup>();
                cg.alpha = 0.0f;
            }    
        }
    }

    public void TogglePredicitonsVisibility(GameObject gameobject)
    {
        predictionsVisible = !predictionsVisible;
        if (predictionsVisible)
        {
            for (int i = 0; i < predictionTexturesList.Count; i++)
            {
                Debug.Log("performing child get ");
                GameObject child = gameobject.transform.GetChild(i).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
                Debug.Log("performed child get ");
                // Reference - https://docs.unity3d.com/6000.3/Documentation/ScriptReference/CanvasGroup.html
                CanvasGroup cg = child.GetComponent<CanvasGroup>();
                cg.alpha = 0.25f;
            }
        }
        else
        {
            for (int i = 0; i < predictionTexturesList.Count; i++)
            {
                Debug.Log("performing child get ");
                GameObject child = gameobject.transform.GetChild(i).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
                Debug.Log("performed child get ");
                // Reference - https://docs.unity3d.com/6000.3/Documentation/ScriptReference/CanvasGroup.html
                CanvasGroup cg = child.GetComponent<CanvasGroup>();
                cg.alpha = 0.0f;
            }    
        }
    }

    public void SizeUp(GameObject gameobject)
    {
        gameobject.transform.localScale = gameobject.transform.localScale * 1.1f;
    }

    public void SizeDown(GameObject gameobject)
    {
        gameobject.transform.localScale = gameobject.transform.localScale * 0.9f;
    }



    public void Incrementer()
    {
        // updates all the image scan views on the panel
        for (int i = 0; i < imageTexturesList.Count ; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            RawImage raws = child.GetComponent<RawImage>();
            raws.texture = imageTexturesList[i][playCounter];
        }
        
        // calls the other Incrementer() methods
        ventricle.GetComponent<VentricleAnimation>().Incrementer(playCounter);
        if (VentricleAnimation.affinesList.Length > 0)
        {
            scanParent.GetComponent<PNGScans>().Incrementer(playCounter, imageTexturesList.Count);
            scanParent2.GetComponent<PredictionScans>().Incrementer(playCounter, predictionTexturesList.Count);    
        }
        // move to next frame with check to see if loop back to start of lists
        playCounter++;
        timer = 0.0f;
        if (playCounter >= count) 
        {
            playCounter = 0;
        }
    }   
}

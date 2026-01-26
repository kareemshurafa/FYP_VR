using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using System.Threading;

public class ImageAnimation : MonoBehaviour
{
    public static List<Texture2D> image1Textures = new List<Texture2D>();
    public static List<Texture2D> image2Textures = new List<Texture2D>();
    public static List<Texture2D> image3Textures = new List<Texture2D>();
    public static List<Texture2D> image4Textures = new List<Texture2D>();
    public static List<Texture2D> image5Textures = new List<Texture2D>();
    public static List<Texture2D> image6Textures = new List<Texture2D>();
    public static bool play;
    public static int playCounter = 0;

    public static int length = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        play = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("length  ===" + image1Textures.Count);
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
        if (play)
        {
            raws1.texture = image1Textures[playCounter];
            raws2.texture = image2Textures[playCounter];
            raws3.texture = image3Textures[playCounter];
            raws4.texture = image4Textures[playCounter];
            raws5.texture = image5Textures[playCounter];
            raws6.texture = image6Textures[playCounter];
            playCounter++;
            if (playCounter >= image3Textures.Count) // temporarily using image3
            {
                playCounter = 0;
            }
        }
    }

    public void build(string pathExtract)
    {
        Debug.Log("Starting animation for images!");
        
        // Reference - https://learn.microsoft.com/en-us/dotnet/api/system.io.directory?view=net-9.0
        string[] pngPaths = Directory.GetFiles(pathExtract, "*.png");
        Debug.Log("Number of pngs: " + pngPaths.Length);

        List<string> first = new List<string>();
        List<string> second = new List<string>();
        List<string> third = new List<string>();
        List<string> fourth = new List<string>();
        List<string> fifth = new List<string>();
        List<string> sixth = new List<string>();
        
        // Reference - https://learn.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex.ismatch?view=net-10.0
        // Reference - https://learn.microsoft.com/en-us/dotnet/api/system.string.contains?view=net-10.0
        foreach (string png in pngPaths)
        {
            if (png.Contains("--1-"))
            {
                first.Add(png);
            }
            else if (png.Contains("--2-"))
            {
                second.Add(png);
            }
            else if (png.Contains("--3-"))
            {
                third.Add(png);
            }
            else if (png.Contains("--5-"))
            {
                fourth.Add(png);
            }
            else if (png.Contains("--6-"))
            {
                fifth.Add(png);
            }
            else if (png.Contains("--7-"))
            {
                sixth.Add(png);
            }
        }
        
        Debug.Log("LENGTH OF FIRST ARRAY: " + first.Count);
        Debug.Log("LENGTH OF SECOND ARRAY: " + second.Count);
        Debug.Log("LENGTH OF THIRD ARRAY: " + third.Count);
        Debug.Log("LENGTH OF FOURTH ARRAY: " + fourth.Count);
        Debug.Log("LENGTH OF FIFTH ARRAY: " + fifth.Count);
        Debug.Log("LENGTH OF SIXTH ARRAY: " + sixth.Count);

        // Reference - https://docs.unity3d.com/6000.2/Documentation/ScriptReference/Windows.File.ReadAllBytes.html
        byte[] image1 = File.ReadAllBytes(first[0]);
        var image2 = File.ReadAllBytes(second[0]);
        var image3 = File.ReadAllBytes(third[0]);
        var image4 = File.ReadAllBytes(fourth[0]);
        var image5 = File.ReadAllBytes(fifth[0]);
        var image6 = File.ReadAllBytes(sixth[0]);
        // the byte[] obtained from using GetType() method
        Debug.Log("Type of image: " + image1.GetType());

        // image1Textures = new List<Texture2D>();
        for (int i = 0; i < third.Count; i++) // temporarily using third.count
        {
            byte[] byteImage1 = File.ReadAllBytes(first[i]);
            Texture2D texImage1 = new Texture2D(2, 2);
            ImageConversion.LoadImage(texImage1, byteImage1);
            image1Textures.Add(texImage1);

            byte[] byteImage2 = File.ReadAllBytes(second[i]);
            Texture2D texImage2 = new Texture2D(2, 2);
            ImageConversion.LoadImage(texImage2, byteImage2);
            image2Textures.Add(texImage2);

            byte[] byteImage3 = File.ReadAllBytes(third[i]);
            Texture2D texImage3 = new Texture2D(2, 2);
            ImageConversion.LoadImage(texImage3, byteImage3);
            image3Textures.Add(texImage3);

            byte[] byteImage4 = File.ReadAllBytes(fourth[i]);
            Texture2D texImage4 = new Texture2D(2, 2);
            ImageConversion.LoadImage(texImage4, byteImage4);
            image4Textures.Add(texImage4);

            byte[] byteImage5 = File.ReadAllBytes(fifth[i]);
            Texture2D texImage5 = new Texture2D(2, 2);
            ImageConversion.LoadImage(texImage5, byteImage5);
            image5Textures.Add(texImage5);

            byte[] byteImage6 = File.ReadAllBytes(sixth[i]);
            Texture2D texImage6 = new Texture2D(2, 2);
            ImageConversion.LoadImage(texImage6, byteImage6);
            image6Textures.Add(texImage6);                   
        }
        Debug.Log("Length of image1Bytes" + image1Textures.Count);
        Debug.Log("Length of image2Bytes" + image2Textures.Count);
        Debug.Log("Length of image3Bytes" + image3Textures.Count);
        Debug.Log("Length of image4Bytes" + image4Textures.Count);
        Debug.Log("Length of image5Bytes" + image5Textures.Count);
        Debug.Log("Length of image6Bytes" + image6Textures.Count);

        // Reference - https://docs.unity3d.com/6000.0/Documentation/ScriptReference/ImageConversion.LoadImage.html
        // the textures will get overwritten, so creating an arbitrary file for now
        Texture2D tex1 = new Texture2D(2, 2);
        Texture2D tex2 = new Texture2D(2, 2);
        Texture2D tex3 = new Texture2D(2, 2);
        Texture2D tex4 = new Texture2D(2, 2);
        Texture2D tex5 = new Texture2D(2, 2);
        Texture2D tex6 = new Texture2D(2, 2);

        // https://docs.unity3d.com/6000.3/Documentation/ScriptReference/ImageConversion.html
        ImageConversion.LoadImage(tex1, image1);
        ImageConversion.LoadImage(tex2, image2);
        ImageConversion.LoadImage(tex3, image3);
        ImageConversion.LoadImage(tex4, image4);
        ImageConversion.LoadImage(tex5, image5);
        ImageConversion.LoadImage(tex6, image6);

        // Reference - https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Transform.GetChild.html
        GameObject child1 = transform.GetChild(0).gameObject;
        GameObject child2 = transform.GetChild(1).gameObject;
        GameObject child3 = transform.GetChild(2).gameObject;
        GameObject child4 = transform.GetChild(3).gameObject;
        GameObject child5 = transform.GetChild(4).gameObject;
        GameObject child6 = transform.GetChild(5).gameObject;

        // Reference - https://docs.unity3d.com/Packages/com.unity.ugui@2.0/manual/script-RawImage.html
        RawImage raw1 = child1.GetComponent<RawImage>();
        raw1.texture = tex1;
        RawImage raw2 =  child2.GetComponent<RawImage>();
        raw2.texture = tex2;
        RawImage raw3 =  child3.GetComponent<RawImage>();
        raw3.texture = tex3;
        RawImage raw4 =  child4.GetComponent<RawImage>();
        raw4.texture = tex4;
        RawImage raw5 =  child5.GetComponent<RawImage>();
        raw5.texture = tex5;
        RawImage raw6 =  child6.GetComponent<RawImage>();
        raw6.texture = tex6;
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
}

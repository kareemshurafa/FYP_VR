using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;

public class ImageAnimation : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        
        // // Reference - https://docs.unity3d.com/6000.2/Documentation/ScriptReference/Windows.File.ReadAllBytes.html
        // var imageData = File.ReadAllBytes(pngPaths[0]);
        
        // // Reference - https://docs.unity3d.com/6000.0/Documentation/ScriptReference/ImageConversion.LoadImage.html
        // Texture2D tex = new Texture2D(2, 2);

        // ImageConversion.LoadImage(tex, imageData);

        // // Reference - https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Transform.GetChild.html
        // GameObject child = transform.GetChild(0).gameObject;
        
        // // Reference - https://docs.unity3d.com/Packages/com.unity.ugui@2.0/manual/script-RawImage.html
        // RawImage raw =  child.GetComponent<RawImage>();
        // raw.texture = tex;

        var image1 = File.ReadAllBytes(first[0]);
        var image2 = File.ReadAllBytes(second[0]);
        var image3 = File.ReadAllBytes(third[0]);
        var image4 = File.ReadAllBytes(fourth[0]);
        var image5 = File.ReadAllBytes(fifth[0]);
        var image6 = File.ReadAllBytes(sixth[0]);

        Texture2D tex1 = new Texture2D(2, 2);
        Texture2D tex2 = new Texture2D(2, 2);
        Texture2D tex3 = new Texture2D(2, 2);
        Texture2D tex4 = new Texture2D(2, 2);
        Texture2D tex5 = new Texture2D(2, 2);
        Texture2D tex6 = new Texture2D(2, 2);

        ImageConversion.LoadImage(tex1, image1);
        ImageConversion.LoadImage(tex2, image2);
        ImageConversion.LoadImage(tex3, image3);
        ImageConversion.LoadImage(tex4, image4);
        ImageConversion.LoadImage(tex5, image5);
        ImageConversion.LoadImage(tex6, image6);

        GameObject child1 = transform.GetChild(0).gameObject;
        GameObject child2 = transform.GetChild(1).gameObject;
        GameObject child3 = transform.GetChild(2).gameObject;
        GameObject child4 = transform.GetChild(3).gameObject;
        GameObject child5 = transform.GetChild(4).gameObject;
        GameObject child6 = transform.GetChild(5).gameObject;

        RawImage raw1 =  child1.GetComponent<RawImage>();
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
}

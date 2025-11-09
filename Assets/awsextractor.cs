using UnityEngine;
using Unity.SharpZipLib.Utils;
using System.IO;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;
using System.Threading;

public class awsextractor : MonoBehaviour
{
    [SerializeField] string resourcespath;

    public GameObject button;

    public GameObject ventricle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Reference - Adapted from: https://docs.unity3d.com/6000.0/Documentation/Manual/web-request-creating-download-handlers.html
    void Start()
    {
        // Coroutine - methods with IEnumerator return types and yield return statement
        // Can stop execution and continue at a later frame - e.g. "yield return null" pauses for one frame (like Update())
        StartCoroutine(DownloadAndExtractFiles());
    }

    IEnumerator DownloadAndExtractFiles()
    {
        // string url = File.ReadAllText(urlpath).Trim();
        var urlfile = Resources.Load<TextAsset>(resourcespath);
        string url = urlfile.ToString().Trim();
        var var1 = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET);
        
        // Reference - https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Application-persistentDataPath.html
        // Need to save files in appropriate file location for VR headset to access at runtime
        string pathZip = Path.Combine(Application.persistentDataPath, "files1.zip");
        var1.downloadHandler = new DownloadHandlerFile(pathZip);
        yield return var1.SendWebRequest();
        if (var1.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(var1.error);
            button.GetComponent<Image>().color = Color.red;
            Thread.Sleep(5000);
        }
        else
            Debug.Log("File successfully downloaded and saved to " + pathZip);
            button.GetComponent<Image>().color = Color.green;
            Thread.Sleep(5000);
            string pathExtract = Path.Combine(Application.persistentDataPath, "ExtractedFiles");
            // Reference - adapted from: https://docs.unity3d.com/Packages/com.unity.sharp-zip-lib@1.4/manual/index.html
            ZipUtility.UncompressFromZip(pathZip, null, pathExtract);
            Thread.Sleep(5000);
            ventricle.GetComponent<VentricleAnimation>().build();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

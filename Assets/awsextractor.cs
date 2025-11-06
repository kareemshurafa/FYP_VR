using UnityEngine;
using Unity.SharpZipLib.Utils;
using System.IO;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;

public class awsextractor : MonoBehaviour
{
    [SerializeField] string resourcespath;

    public GameObject button;

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
        string path = Path.Combine(Application.persistentDataPath, "files.zip");
        var1.downloadHandler = new DownloadHandlerFile(path);
        yield return var1.SendWebRequest();
        if (var1.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(var1.error);
            button.GetComponent<Image>().color = Color.red;
        }
        else
            Debug.Log("File successfully downloaded and saved to " + path);
            button.GetComponent<Image>().color = Color.green;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

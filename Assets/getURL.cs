using UnityEngine;
using Unity.SharpZipLib.Utils;
using System.IO;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;
using System.Threading;
using Dummiesman;
using TMPro;

public class getURL : MonoBehaviour
{
    [SerializeField] string appURL;

    // Reference - adapted from ChatGPT 5.0
    public TMP_InputField nameText;
    public TMP_InputField passwordText;

    public GameObject ventricle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Reference - Adapted from: https://docs.unity3d.com/2022.3/Documentation/Manual/UnityWebRequest-CreatingDownloadHandlers.html
    void Start()
    {
        // Coroutine - methods with IEnumerator return types and yield return statement
        // Can stop execution and continue at a later frame - e.g. "yield return null" pauses for one frame (like Update())
        StartCoroutine(GetURLAndExtract());
    }

    IEnumerator GetURLAndExtract()
    {
        // Reference - https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Networking.UnityWebRequest.Post.html
        // Reference - https://learn.unity.com/tutorial/working-with-textmesh-pro
        string name = nameText.text;
        string pass = passwordText.text;
        Debug.Log("Name:" + name + "Pass:" + pass);
        using (UnityWebRequest request = UnityWebRequest.Post(appURL, "{ \"objectName\": \"" + name + "\", \"password\": \"" + pass + "\" }", "application/json"))
        {
           yield return request.SendWebRequest();

           if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Failed to get URL with error code: " + request.downloadHandler.text);
                GetComponent<Image>().color = Color.red;
            }
            else
            {
                Debug.Log("Obtained URL successfuly");
                GetComponent<Image>().color = Color.green;
                Debug.Log(request.downloadHandler.text);
                StartCoroutine(DownloadAndExtractFiles(request.downloadHandler.text));
            }
        }
    }

    IEnumerator DownloadAndExtractFiles(string URL) 
    {
        // string url = File.ReadAllText(urlpath).Trim();
        // var urlfile = Resources.Load<TextAsset>(resourcespath);
        // string url = urlfile.ToString().Trim();
        var var1 = new UnityWebRequest(URL, UnityWebRequest.kHttpVerbGET);
        
        // Reference - https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Application-persistentDataPath.html
        // Need to save files in appropriate file location for VR headset to access at runtime
        string pathZip = Path.Combine(Application.persistentDataPath, "files1.zip");
        var1.downloadHandler = new DownloadHandlerFile(pathZip);
        yield return var1.SendWebRequest();
        if (var1.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(var1.error);
            // button.GetComponent<Image>().color = Color.red;
            //Thread.Sleep(5000);
        }
        else
        {
            Debug.Log("File successfully downloaded and saved to " + pathZip);
            // button.GetComponent<Image>().color = Color.green;
            Thread.Sleep(3000);
            string pathExtract = Path.Combine(Application.persistentDataPath, "ExtractedFiles");
            // Reference - adapted from ChatGPT-5 (OpenAI)
            Directory.CreateDirectory(pathExtract);
            // Reference - adapted from: https://docs.unity3d.com/Packages/com.unity.sharp-zip-lib@1.4/manual/index.html
            ZipUtility.UncompressFromZip(pathZip, null, pathExtract);
            //Thread.Sleep(5000);
            Debug.Log("Now calling animation method!");
            ventricle.GetComponent<VentricleAnimation>().build(pathExtract);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

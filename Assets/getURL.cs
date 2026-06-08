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
    [SerializeField] public TMP_InputField nameText;
    [SerializeField] public TMP_InputField passwordText;

    public GameObject ventricle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Coroutine - methods with IEnumerator return types and yield return statement
        // can stop execution and continue at a later frame - e.g. "yield return null" pauses for one frame
        StartCoroutine(GetURLAndExtract());
    }

    IEnumerator GetURLAndExtract()
    {
        // Reference - https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Networking.UnityWebRequest.Post.html
        // Reference - https://learn.unity.com/tutorial/working-with-textmesh-pro
        // taking values from user input
        string name = nameText.text;
        string pass = passwordText.text;
        Debug.Log("Name: " + name + "Pass: " + pass);
        
        // creating the POST request object - appURL contains the website URL
        using (UnityWebRequest request = UnityWebRequest.Post(appURL, "{ \"objectName\": \"" + name + "\", \"password\": \"" + pass + "\" }", "application/json"))
        {
           // sending POST request
           yield return request.SendWebRequest();
           // check for failure 
           if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Failed to get URL with error: " + request.downloadHandler.text);
                // sets button red to tell user of failure
                GetComponent<Image>().color = Color.red;
            }
            else
            {
                Debug.Log("Obtained pre-signed URL successfuly");
                // sets button green to tell user of success
                GetComponent<Image>().color = Color.green;
                Debug.Log(request.downloadHandler.text);
                // calling the VentricleAnimation.cs script and passing URL
                StartCoroutine(ventricle.GetComponent<VentricleAnimation>().DownloadAndExtractFiles(request.downloadHandler.text));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

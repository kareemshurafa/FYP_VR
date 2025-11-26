using UnityEngine;
using System.Collections.Generic;

public class LoadAnimation : MonoBehaviour
{

    List<Mesh> meshList;
    int counter = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Getting meshList!");
        meshList = GetComponent<VentricleAnimation>().meshList;
        Debug.Log("Now calling meshList update!");
        
        Debug.Log("Position" + transform.position);
        Debug.Log("Scale: " + transform.localScale);
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<MeshFilter>().mesh = meshList[counter];
        counter++;
        if (counter == meshList.Count)
        {
            Debug.Log("Resetting counter!");
            counter = 0;
        }
    }
}

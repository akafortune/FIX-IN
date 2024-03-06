using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading : MonoBehaviour
{
    private GameObject canvas;
    private GameObject loadScreen;
    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas");
        loadScreen = GameObject.Find("Load");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private GameObject canvas;
    private GameObject loadScreen;
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(0))
        {
            canvas = GameObject.Find("Canvas");
            loadScreen = GameObject.Find("Load");
            loadScreen.SetActive(false);
        }
    }
    public void LoadScene(int scene)
    {
        if(scene == 3)
        {
            canvas.SetActive(false);
            loadScreen.SetActive(true);
            loadScreen.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }    
        SceneManager.LoadScene(scene); 
    }

    public void Quit()
    {
        Application.Quit();
    }

}

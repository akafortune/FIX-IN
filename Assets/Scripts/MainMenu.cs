using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    private GameObject canvas;
    private GameObject loadScreen;
    public float buffer = 0f;
    public bool buttonPressed;
    public bool sceneChange;
    public int sceneNumber;
    public GameObject SettingsMenu;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        buttonPressed = false;
        sceneChange = false;
        buffer = 0f;
        audioSource = GetComponentInParent<AudioSource>();
        //hammerIndicator = GameObject.Find("Indicator").GetComponent<GameObject>();
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(0))
        {
            canvas = GameObject.Find("Canvas");
            loadScreen = GameObject.Find("Load");
            loadScreen.SetActive(false);
        }
    }

    public void Update()
    {
        if (buttonPressed)
        {
            buffer += Time.deltaTime;
            print((int)buffer);
        }
        if (buffer >= 0.4f)
        {
            buttonPressed = false;
            sceneChange = true;
            LoadScene(sceneNumber);
        }
    }

    public void LoadScene(int scene)
    {
        buffer = 0f;
        buttonPressed = true;
        sceneNumber = scene;
        if (sceneChange)
        {
            print("Change scene now");
            if (scene == 3)
            {
                canvas.SetActive(false);
                loadScreen.SetActive(true);
                loadScreen.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            }
            SceneManager.LoadScene(scene);
            sceneChange = false;
        }
    }

    public void ToggleSettings()
    {
        if(!SettingsMenu.activeInHierarchy)
        {
            SettingsMenu.SetActive(true);
        }
        else
        {
            SettingsMenu.SetActive(false);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}

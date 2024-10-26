using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public GameObject canvas;
    public GameObject loadScreen;
    public static GameObject greenGuy;
    public float buffer = 0f;
    public bool buttonPressed;
    public bool sceneChange;
    public int sceneNumber;
    public GameObject SettingsMenu;
    SettingsManager settingsManager;
    private AudioSource audioSource;
    public AudioClip gameOverSound, risingPoints;

    public TextMeshProUGUI roundCountText;
    public TextMeshProUGUI pointValueText;
    int scoreStart = 0;

    public GameObject crossfade;

    // Start is called before the first frame update
    void Start()
    {
        settingsManager = SettingsMenu.GetComponent<SettingsManager>();
        settingsManager.init();
        //crossfade = GameObject.Find("Crossfade");
        buttonPressed = false;
        sceneChange = false;
        buffer = 0f;
        audioSource = GetComponentInParent<AudioSource>();
        //hammerIndicator = GameObject.Find("Indicator").GetComponent<GameObject>();
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(0) || SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(6) || SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(7))
        {
            canvas = GameObject.Find("Canvas");
            loadScreen = GameObject.Find("Load");
            loadScreen.SetActive(false);
        }
        if (SceneManager.GetActiveScene().name == "GameOver")
        {
            audioSource.PlayOneShot(gameOverSound);
            roundCountText = GameObject.Find("RoundCountText").GetComponent<TextMeshProUGUI>();
            pointValueText = GameObject.Find("ScoreNumberText").GetComponent<TextMeshProUGUI>();
            roundCountText.text = GameStats.roundsLasted.ToString();
            pointValueText.text = scoreStart.ToString();
        }
        greenGuy = GameObject.Find("GreenGent");
    }

    public void Update()
    {
        /*if (buttonPressed)
        {
            print("Button has been pressed");
            buffer += Time.unscaledDeltaTime;
        }
        if (buffer >= 0.4f)
        {
            buttonPressed = false;
            sceneChange = true;
            LoadScene(sceneNumber);
        }*/
        
        if (SceneManager.GetActiveScene().name == "GameOver")
        {
            pointValueText.text = scoreStart.ToString();
            if (scoreStart < GameStats.playerScore)
            {
                scoreStart++;
                if(risingPoints != null)
                {
                    audioSource.PlayOneShot(risingPoints);
                }
            }
        }
    }

    public void LoadScene(int scene)
    {
        /*buffer = 0f;
        buttonPressed = true;*/
        sceneChange = true;
        sceneNumber = scene;
        if (sceneChange)
        {
            print("Change scene now");
            if (scene == 3)
            {
                Debug.Log("CrossfadeStart");
                crossfade.GetComponent<SceneTransition>().LoadLevelTransition(scene, "load");
            }
            //SceneManager.LoadScene(scene);
            else
            {
                crossfade.GetComponent<SceneTransition>().LoadLevelTransition(scene, "");
            }
            sceneChange = false;
        }
    }

    public void ToggleSettings()
    {
        if(!SettingsMenu.activeInHierarchy)
        {
            StartCoroutine(waitToToggleSettings());
        }
        else
        {
            SettingsMenu.SetActive(false);
        }
    }

    private IEnumerator waitToToggleSettings()
    {
        yield return new WaitForSeconds(.25f);
        SettingsMenu.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}

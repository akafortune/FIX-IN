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
    private GameObject loadScreen;
    public float buffer = 0f;
    public bool buttonPressed;
    public bool sceneChange;
    public int sceneNumber;
    private AudioSource audioSource;
    public AudioClip gameOverSound, risingPoints;

    public TextMeshProUGUI roundCountText;
    public TextMeshProUGUI pointValueText;
    int scoreStart = 0;

    // Start is called before the first frame update
    void Start()
    {
        buttonPressed = false;
        sceneChange = false;
        buffer = 0f;
        audioSource = GetComponentInParent<AudioSource>();
        //hammerIndicator = GameObject.Find("Indicator").GetComponent<GameObject>();
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(0) || SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(6))
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
    }

    public void Update()
    {
        if (buttonPressed)
        {
            print("Button has been pressed");
            buffer += Time.unscaledDeltaTime;
            print(buffer);
        }
        if (buffer >= 0.4f)
        {
            buttonPressed = false;
            sceneChange = true;
            LoadScene(sceneNumber);
        }
        
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

    public void Quit()
    {
        Application.Quit();
    }
}

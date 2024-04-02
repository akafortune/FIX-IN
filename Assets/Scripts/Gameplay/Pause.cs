using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Pause : MonoBehaviour
{
    public GameObject pauseMenu;
    private GameObject StartButton, RebuildButton;
    public AudioSource songSource;
    public bool rebuildActive;
    //public bool buttonPressed;
    public float buffer = 0f;
    public bool restartGame;
    public bool pressRestart;
    public bool sceneChange;
    public bool pressQuit;
    public SelectedButton restartButton;
    public bool pauseStart;
    bool sceneStart;

    // Start is called before the first frame update
    void Awake()
    {
        songSource = GameObject.Find("SongSource").GetComponent<AudioSource>();
        pauseMenu = GameObject.Find("PauseMenu");
        StartButton = GameObject.Find("Start Button");
        RebuildButton = GameObject.Find("Rebuild Button");
        GameObject.Find("MusicSource");
        //buttonPressed = false;
        restartGame = false;
        pressRestart = false;
        sceneChange = false;
        pressQuit = false;
        buffer = 0f;
        sceneStart = true;
        //restartButton = GameObject.Find("RestartButton").GetComponent<SelectedButton>();
    }

    // Update is called once per frame
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)||Input.GetKeyDown("joystick button 7"))
        {
            if (pauseMenu.activeSelf)
            {
                pauseMenu.SetActive(false);
                //StartButton.SetActive(true);
                if(rebuildActive)
                    RebuildButton.SetActive(true);
                Time.timeScale = 1f;
                songSource.Play();
            }
            else
            {
                pauseMenu.SetActive(true);
                //StartButton.SetActive(false);
                rebuildActive = RebuildButton.activeInHierarchy;
                RebuildButton.SetActive(false);
                Time.timeScale = 0f;
                songSource.Pause();
                //pauseStart = true;
            }
        }
        if (pressRestart || pressQuit)
        {
            //Time.timeScale = 1f;
            buffer += Time.unscaledDeltaTime;
            print(buffer);
        }
        if (buffer >= 0.4f)
        {
            //buttonPressed = false;
            if(pressRestart)
            {
                restartGame = true;
                Restart();
                pressRestart = false;
            }
            else if(pressQuit)
            {
                sceneChange = true;
                Quit();
                pressQuit = false;
            }
        }
        /*if (pauseStart)  BRUTE FORCE WHY DID YOU FAIL ME
        {
            restartButton.BackupSelect();
            print(restartButton.GetIsSelected());

            if (restartButton.GetIsSelected())
            {
                pauseStart = false;
            }
        }*/
        if (sceneStart)
        {
            pauseMenu.SetActive(false);
            sceneStart = false;
        }    
    }


    public void Restart()
    {
        pressRestart = true;
        if (restartGame)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Time.timeScale = 1f;
        }
    }

    public void NewTest()
    {
        Restart();
    }

    public void Quit()
    {
        pressQuit = true;
        if (sceneChange)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(0);
        }
    }
}

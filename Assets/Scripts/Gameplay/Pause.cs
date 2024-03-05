using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject pauseMenu;
    private GameObject StartButton;
    public AudioSource songSource;
    // Start is called before the first frame update
    void Start()
    {
        pauseMenu = GameObject.Find("PauseMenu");
        StartButton = GameObject.Find("Start Button");
        GameObject.Find("MusicSource");
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.activeSelf)
            {
                pauseMenu.SetActive(false);
                StartButton.SetActive(true);
                Time.timeScale = 1f;
                songSource.Play();
            }
            else
            {
                pauseMenu.SetActive(true);
                StartButton.SetActive(false);
                Time.timeScale = 0f;
                songSource.Pause();
            }
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }

    public void NewTest()
    {
        Restart();
    }

    public void Quit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}

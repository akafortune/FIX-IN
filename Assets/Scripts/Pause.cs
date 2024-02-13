using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject pauseMenu;

    // Start is called before the first frame update
    void Start()
    {
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
                Time.timeScale = 1f;
            }
            else
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0f;
            }
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene("scene");
        Time.timeScale = 1f;
    }

    public void NewTest()
    {
        Ball.newAugment = true;
        Restart();
    }

    public void Quit()
    {
        Debug.Log("Game will quit");
        Application.Quit();
    }
}

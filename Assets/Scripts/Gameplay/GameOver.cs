using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public GameObject gameOverMenu;
    public GameObject[] otherUI;
    private GameObject gameOverBackground;

    // Start is called before the first frame update
    void Start()
    {
        gameOverBackground = GameObject.Find("Game_Over_GG");
        gameOverBackground.SetActive(false);
        gameOverMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(gameOverMenu.activeSelf)
        {
            gameOverBackground.SetActive(true);
            foreach(GameObject uiObject in otherUI)
            {
                uiObject.SetActive(false);
            }
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }

    public void Quit()
    {
        SceneManager.LoadScene(0);
    }
}

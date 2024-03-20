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
            gameOverBackground.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            foreach(GameObject uiObject in otherUI)
            {
                uiObject.SetActive(false);
            }
        }
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        SceneManager.LoadScene(0);
    }
}

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
    public void Retry()
    {
        Time.timeScale = 1f;
        string scene = SceneManager.GetActiveScene().name;
        SceneManager.UnloadSceneAsync(scene);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        SceneManager.LoadScene(0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EventSystemManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject gameOverMenu;
    public GameObject pauseEventSystem;
    public GameObject gameOverEventSystem;
    //public Button selectedButton;
    //public EventSystem pauseES;

    // Start is called before the first frame update
    void Start()
    {
        gameOverEventSystem.SetActive(false);
    }

    private void Awake()
    {
        pauseMenu = GameObject.Find("PauseMenu");
        gameOverMenu = GameObject.Find("GameOverMenu");
        pauseEventSystem = GameObject.Find("PauseEventSystem");
        gameOverEventSystem = GameObject.Find("GameOverEventSystem");
        //pauseES = pauseEventSystem.GetComponent<EventSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if(pauseMenu.activeInHierarchy)
        {
            pauseEventSystem.SetActive(true);
            gameOverEventSystem.SetActive(false);
        }
        else if(gameOverMenu.activeInHierarchy)
        {
            gameOverEventSystem.SetActive(true);
            pauseEventSystem.SetActive(false);
        }

    }

    private void FixedUpdate()
    {
        if (pauseEventSystem.activeInHierarchy)
        {
            gameOverEventSystem.SetActive(false);
            //pauseES = GameObject.Find("PauseEventSystem").GetComponent<EventSystem>();
            //pauseES.SetSelectedGameObject(selectedButton.gameObject);
            //pauseES.firstSelectedGameObject = selectedButton.gameObject;
        }
        else if (gameOverEventSystem.activeInHierarchy)
        {
            pauseEventSystem.SetActive(false);
        }
    }
}

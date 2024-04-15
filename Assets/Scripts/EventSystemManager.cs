using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EventSystemManager : MonoBehaviour
{
    //public GameObject otherMenu;
    public GameObject settingsMenu;
    public GameObject otherEventSystem;
    public GameObject settingsEventSystem;
    //public Button selectedButton;
    //public EventSystem pauseES;

    // Start is called before the first frame update
    void Start()
    {
        settingsMenu.SetActive(false);
    }

    private void Awake()
    {
        settingsMenu = GameObject.Find("SettingsMenu");
        settingsEventSystem = GameObject.Find("OptionsEventSystem");
    }

    // Update is called once per frame
    void Update()
    {
        if(settingsMenu.activeInHierarchy)
        {
            otherEventSystem.SetActive(false);
            settingsEventSystem.SetActive(true);
            //gameOverEventSystem.SetActive(false);
        }
        else
        {
            settingsEventSystem.SetActive(false);
            otherEventSystem.SetActive(true);
            //settingsMenu.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (otherEventSystem.activeInHierarchy)
        {
            //pauseES = GameObject.Find("PauseEventSystem").GetComponent<EventSystem>();
            //pauseES.SetSelectedGameObject(selectedButton.gameObject);
            //pauseES.firstSelectedGameObject = selectedButton.gameObject;
        }
    }
}

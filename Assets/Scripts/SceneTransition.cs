using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    private Animator transition;
    // Start is called before the first frame update
    void Start()
    {
        transition = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void LoadLevelTransition(int scene, string arg)
    {
        if (arg == "")
        {        
            transition.SetTrigger("Start");
            StartCoroutine(LoadLevelTransitionCR(scene, 1f));
        }
        else if (arg == "load")
        {
            Debug.Log("Got Arg");
            GameObject canvas = GameObject.Find("Canvas");
            GameObject load = canvas.GetComponent<MainMenu>().loadScreen;
            Debug.Log(canvas.name);
            Debug.Log(load.name);
            StartCoroutine(DelayLoad(canvas, load));
            StartCoroutine(LoadLevelTransitionCR(scene, 3f));
        }
        
    }

    IEnumerator DelayLoad(GameObject canvas, GameObject load)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        canvas.SetActive(false);
        load.SetActive(true);
        load.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        transition.gameObject.GetComponent<Canvas>().enabled = false;
    }

    IEnumerator LoadLevelTransitionCR(int scene, float time)
    {
        yield return new WaitForSeconds(time);

        SceneManager.LoadScene(scene);
    }
}

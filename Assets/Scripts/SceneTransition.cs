using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void LoadLevelTransition(int scene)
    {
        StartCoroutine(LoadLevelTransitionCR(scene));
    }

    IEnumerator LoadLevelTransitionCR(int scene)
    {
        transition.SetTrigger("Start");


        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(scene);
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectedButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public bool isSelected { get; private set; } = false;

    public GameObject hammerIndicatorParent;
    public Animator[] hammerAnims;
    public AudioSource audioSource;
    public AudioClip buttonChange;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponentInParent<AudioSource>();
        hammerAnims = hammerIndicatorParent.GetComponentsInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isSelected)
        {
            hammerIndicatorParent.SetActive(true);
            
            if(Input.GetKeyDown(KeyCode.Mouse0) || Input.GetButtonDown("Submit"))
            {
                foreach(Animator anim in hammerAnims)
                {
                    anim.SetTrigger("ButtonSelected");
                }
            }
        }
        if (!isSelected)
        {
            hammerIndicatorParent.SetActive(false);
        }
    }

    public void OnSelect(BaseEventData eData)
    {
        isSelected = true;
        audioSource.PlayOneShot(buttonChange);
    }

    public void OnDeselect(BaseEventData eData) 
    {
        isSelected = false;
    }
}

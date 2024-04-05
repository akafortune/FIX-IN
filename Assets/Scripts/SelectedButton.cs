using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectedButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public bool isSelected;

    public GameObject hammerIndicatorParent;
    public Animator[] hammerAnims;
    public AudioSource audioSource;
    public AudioClip buttonChange;
    public AudioClip buttonPress;

    // Start is called before the first frame update
    void Start()
    {
        isSelected = false;
        audioSource = GetComponentInParent<AudioSource>();
        hammerAnims = hammerIndicatorParent.GetComponentsInChildren<Animator>();
        //if (this.gameObject.name == "RestartButton")
        //{
        //    BackupSelect();
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if(isSelected)
        {
            hammerIndicatorParent.SetActive(true);
            //print("Hammers are showing");
            
            if(Input.GetKeyDown(KeyCode.Mouse0) || Input.GetButtonDown("Submit"))
            {
                audioSource.PlayOneShot(buttonPress);
                foreach (Animator anim in hammerAnims)
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

    void FixedUpdate()
    {
        foreach (Animator anim in hammerAnims)
        {
            anim.ResetTrigger("ButtonSelected");
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

    public void BackupSelect()
    {
        isSelected = true;
        audioSource.PlayOneShot(buttonChange);
    }

    public bool GetIsSelected()
    {
        return isSelected;
    }
}

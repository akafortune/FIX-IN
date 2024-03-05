using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public class Brick : MonoBehaviour
{
    Animator animator;
    BoxCollider2D bc;
    public AudioSource audioSource;
    public AudioClip brickBreak;
    public SpriteRenderer fixIndicator;
    private static bool canBreak;
    // Start is called before the first frame update
    void Awake()
    {
        fixIndicator = GetComponentsInChildren<SpriteRenderer>()[1];
        audioSource = GetComponentInParent<AudioSource>();
        animator = GetComponent<Animator>();
        bc = GetComponent<BoxCollider2D>();
        fixIndicator.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        fixIndicator.enabled = false;
        canBreak = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("collision");
            if (collision.gameObject.tag.Equals("Ball") && canBreak)
            {
                canBreak = false;
                animator.SetBool("IsBroken", true);
                bc.isTrigger = true;
                audioSource.clip = brickBreak;
                audioSource.Play();
                BaseBuilding.resources += this.transform.parent.GetComponent<BrickLayers>().brickValue;
            }
    }

    public void fixBrick()
    {
        animator.SetBool("IsBroken", false);
        bc.isTrigger = false;
    }

    public void cancelBrick()
    {
        animator.SetTrigger("CancelFix");
        animator.SetBool("IsBroken", true);
        bc.isTrigger = true;
    }

    public void ShowIndicator()
    {
        fixIndicator.enabled = true;
    }

    public void StartForBuild()
    {
        canBreak = false;
        animator.SetBool("IsBroken", true);
        bc.isTrigger = true;
        animator.Play("BrokenBrick", 0, 0);
    }
}

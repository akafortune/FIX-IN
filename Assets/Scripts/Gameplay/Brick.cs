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
    private SpriteRenderer spriteRenderer;
    public SpriteRenderer fixIndicator;
    public SpriteRenderer breakIndicator;
    public GameObject[] brickTypes;
    private static bool canBreak;
    private GreenGuy GreenGuy;
    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        fixIndicator = GetComponentsInChildren<SpriteRenderer>()[1];
        breakIndicator = GetComponentsInChildren<SpriteRenderer>()[2];
        audioSource = GetComponentInParent<AudioSource>();
        animator = GetComponent<Animator>();
        bc = GetComponent<BoxCollider2D>();
        fixIndicator.enabled = false;
        breakIndicator.enabled = false;
        GreenGuy = GameObject.Find("GreenGuy").GetComponent<GreenGuy>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        fixIndicator.enabled = false;
        breakIndicator.enabled = false;
        canBreak = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("collision");
            if (collision.gameObject.tag.Equals("Ball") && canBreak)
            {
                canBreak = false;
                Ball.hits++;
                animator.SetBool("IsBroken", true);
                bc.isTrigger = true;
                audioSource.PlayOneShot(brickBreak);
                //BaseBuilding.resources += this.GetComponentInParent<BrickLayers>().brickValue;
            }
    }

    public void fixBrick()
    {
        animator.SetBool("IsBroken", false);
        bc.isTrigger = false;
    }

    public void specialBrick(int brickIndex)
    {
        GameObject p = Instantiate(brickTypes[brickIndex-1], this.transform);
        bc.enabled = false;
        spriteRenderer.enabled = false;
        SpecialTile tile = p.GetComponent<SpecialTile>();
        tile.Brick = this;
        tile.index = brickIndex;
    }

    public void removeSpecialBrick(int index)
    {
        bc.enabled = true;
        spriteRenderer.enabled = true;
        GreenGuy.specialBrickAmounts[index]++;
    }

    public void cancelBrick()
    {
        animator.SetTrigger("CancelFix");
        animator.SetBool("IsBroken", true);
        bc.isTrigger = true;
    }

    public void ShowFixIndicator()
    {
        fixIndicator.enabled = true;
    }

    public void ShowBreakIndicator()
    {
        breakIndicator.enabled = true;
    }

    public void StartForBuild()
    {
        canBreak = false;
        animator.SetBool("IsBroken", true);
        bc.isTrigger = true;
        animator.Play("BrokenBrick", 0, 0);
    }

    public bool isBuilt()
    {
        return !animator.GetBool("IsBroken");
    }
}

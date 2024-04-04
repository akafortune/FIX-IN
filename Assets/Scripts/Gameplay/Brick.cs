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
    public GameObject SpecialBrick;
    public static bool canBreak;
    public bool replaced;
    private GreenGuy GreenGuy;
    BaseBuilding baseBuilding;
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
        replaced = false;
        baseBuilding = GameObject.FindAnyObjectByType<BaseBuilding>();
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
        baseBuilding.checkWin();
    }

    public void PlaceSpecialBrick(int brickIndex)
    {
        replaced = true;
        if (baseBuilding.checkWin())
        {
            replaced = false;
            return;
        }
        SpecialBrick = Instantiate(brickTypes[brickIndex-1], this.transform);
        bc.enabled = false;
        SpecialTile tile = SpecialBrick.GetComponent<SpecialTile>();
        tile.associatedBrick = this;
        tile.index = brickIndex;
        bc.isTrigger = false;
        spriteRenderer.enabled = false;
    }

    public void removeSpecialBrick(int index)
    {
        replaced = false;
        bc.isTrigger = true;
        bc.enabled = true;
        spriteRenderer.enabled = true;
        GreenGuy.specialBrickAmounts[index]++;
    }

    public void removeSpecialBrick()
    {
        replaced = false;
        bc.isTrigger = true;
        bc.enabled = true;
        spriteRenderer.enabled = true;
    }

    public void cancelBrick()
    {
        animator.SetTrigger("CancelFix");
        animator.SetBool("IsBroken", true);
        bc.isTrigger = true;
    }

    public void ShowFixIndicator()
    {
        if(!replaced)
            fixIndicator.enabled = true;
    }

    public void ShowBreakIndicator()
    {
        if(!replaced)
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    Animator animator;
    BoxCollider2D bc; 
    public AudioSource audioSource;
    public AudioClip brickBreak;
    public SpriteRenderer fixIndicator;
    static bool canBreak;
    // Start is called before the first frame update
    void Start()
    {
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
            animator.SetBool("IsBroken", true);
            bc.isTrigger = true;
            audioSource.clip = brickBreak;
            audioSource.Play();
            canBreak = false;
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
}

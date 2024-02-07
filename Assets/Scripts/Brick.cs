using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    Animator animator;
    BoxCollider2D bc; 
    public AudioSource audioSource;
    public AudioClip brickBreak;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        bc = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("collision");
        if (collision.gameObject.tag.Equals("Ball"))
        {
            animator.SetBool("IsBroken", true);
            bc.isTrigger = true;
            audioSource.clip = brickBreak;
            audioSource.Play();
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
}

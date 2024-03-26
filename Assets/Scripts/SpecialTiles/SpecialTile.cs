using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialTile : MonoBehaviour
{
    protected bool effectActive;
    protected float timeStart;
    protected float effectLength;
    protected Rigidbody2D rb;
    public Brick Brick;
    public int index;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        effectActive = false;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.SetBool("IsBroken", false);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void Update()
    {
        rb.AddForce(Vector2.zero);
        if (Time.time > timeStart + effectLength && effectActive)
        {
            stopAction();
            effectActive = false;
        }

        if(!spriteRenderer.enabled)
        {
           Destroy(gameObject);
        }
    }

    // Update is called once per frame
    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("SpecialBrickTrigger") && !effectActive && GreenGuy.canJump)
        {
            timeStart = Time.time;
            doAction();
            effectActive=true;
        }
    }

    protected virtual void doAction() { }
    protected virtual void stopAction() { }

    protected virtual void cancelBrick()
    {
        Brick.removeSpecialBrick(index-1);
        animator.SetBool("IsBroken", true);
        //Destroy(this.gameObject);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Ball"))
        {
            animator.SetBool("IsBroken", true);
            Ball.hits++;
            Brick.removeSpecialBrick();
            //Destroy(this.gameObject);
        }
    }
}

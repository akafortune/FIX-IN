using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialTile : MonoBehaviour
{
    protected bool effectActive;
    protected float timeStart;
    protected float effectLength;
    protected Rigidbody2D rb;
    public Brick associatedBrick;
    public int index;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    public SpriteRenderer breakIndicator;
    protected bool broken;
    protected BoxCollider2D BoxCollider;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        effectActive = false;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.SetBool("IsBroken", false);
        spriteRenderer = GetComponent<SpriteRenderer>();
        breakIndicator = transform.GetChild(transform.childCount - 1).GetComponent<SpriteRenderer>();
        BoxCollider = GetComponent<BoxCollider2D>();
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
            if(effectActive)
                stopAction();
           Destroy(gameObject);
        }
    }

    protected virtual void FixedUpdate()
    {
        breakIndicator.enabled = false;
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
        associatedBrick.removeSpecialBrick(index-1);
        animator.SetBool("IsBroken", true);
        //Destroy(this.gameObject);
        broken = true;
        BoxCollider.enabled = false;
    }

    public virtual void RemoveBrick()
    {
        associatedBrick.removeSpecialBrick();
        animator.SetBool("IsBroken", true);
        //Destroy(this.gameObject);
        broken = true;
        BoxCollider.enabled = false;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Ball") && Brick.canBreak)
        {
            Brick.canBreak = false;
            animator.SetBool("IsBroken", true);
            Ball.hits++;
            associatedBrick.removeSpecialBrick();
            BoxCollider.enabled = false;
            //Destroy(this.gameObject);
        }
    }

    public void ShowBreakIndicator()
    {
        if(!broken)
            breakIndicator.enabled=true;
    }
}

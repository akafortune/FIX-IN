using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reinforced : SpecialTile
{
    int hits;
    SpriteRenderer repairIndicator;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        hits = 3;
        animator.SetInteger("HP", 3);
        animator.SetBool("IsBroken", false);
        repairIndicator = transform.GetChild(0).GetComponent<SpriteRenderer>(); 
    }

    // Update is called once per frame
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        bool ballHit = collision.gameObject.name.Equals("Ball");

        if (ballHit && Brick.canBreak)
        {
            hits--;
            animator.SetInteger("HP", hits);
            if (hits <= 0)
            {
                animator.SetBool("IsBroken", true);
                base.OnCollisionEnter2D(collision);
                associatedBrick.removeSpecialBrick();
            }
            Brick.canBreak = false;
        }
    }

    protected override void FixedUpdate()
    {
        repairIndicator.enabled = false;
        base.FixedUpdate();
    }
    public void ResetHits()
    {
        hits = 3;
        animator.SetInteger("HP", hits);
    }

    public bool canRepair()
    {
        return (0 < hits && hits < 3) ? true : false;
    }

    public void repair()
    {
        hits++;
        StartCoroutine(repairAnimator());
    }

    public void ShowRepair()
    {
        if(canRepair())
            repairIndicator.enabled = true;
    }    

    protected IEnumerator repairAnimator()
    {
        yield return new WaitForSeconds(.4f);
        animator.SetInteger("HP", hits);
    }    
}

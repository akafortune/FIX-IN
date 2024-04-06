using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reinforced : SpecialTile
{
    int hits;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        hits = 3;
        animator.SetInteger("HP", 3);
        animator.SetBool("IsBroken", false);
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

    public void ResetHits()
    {
        hits = 3;
        animator.SetInteger("HP", hits);
    }
}

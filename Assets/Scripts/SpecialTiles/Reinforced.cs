using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reinforced : SpecialTile
{
    int hits;
    // Start is called before the first frame update
    protected override void Start()
    {
        hits = 3;
        base.Start();
    }

    // Update is called once per frame
    protected override void cancelBrick()
    {
        base.cancelBrick();
        Destroy(this.gameObject);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        bool ballHit = collision.gameObject.name.Equals("Ball");

        if (ballHit && Brick.canBreak)
        {
            hits--;
            if (hits <= 0)
            {
                base.OnCollisionEnter2D(collision);
                Destroy(this.gameObject);
            }
            Brick.canBreak = false;
        }
           
        if (hits <= 0 && ballHit)
        {
            
        }
    }
}

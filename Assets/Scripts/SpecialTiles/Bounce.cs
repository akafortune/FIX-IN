using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : SpecialTile
{
    GameObject greenGuy;
    Rigidbody2D greenGuyRB;
    BoxCollider2D[] platforms;
    // Start is called before the first frame update
    void Start()
    {
        greenGuy = GameObject.Find("GreenGuy");
        greenGuyRB = greenGuy.GetComponent<Rigidbody2D>();
        effectLength = .1f;
        platforms = GameObject.Find("Platforms").GetComponentsInChildren<BoxCollider2D>();
    }

    protected override void Update()
    {
        if ((Time.time > timeStart + effectLength && effectActive) || (effectActive && greenGuyRB.velocity.y < 0))
        {
            stopAction();
            effectActive = false;
        }
    }
    protected override void doAction()
    {
        greenGuy.SendMessage("Bounce");
    }

    protected override void stopAction()
    {
        foreach (BoxCollider2D platform in platforms)
        {
            platform.enabled = true;
        }
    }
}

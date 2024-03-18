using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : SpecialTile
{
    GameObject greenGuy;
    BoxCollider2D[] platforms;
    // Start is called before the first frame update
    void Start()
    {
        greenGuy = GameObject.Find("GreenGuy");
        effectLength = .1f;
        platforms = GameObject.Find("Platforms").GetComponentsInChildren<BoxCollider2D>();
    }

    protected override void doAction()
    {
        greenGuy.SendMessage("Bounce");
        foreach (BoxCollider2D platform in platforms)
        {
            platform.enabled = false;
        }
    }

    protected override void stopAction()
    {
        foreach (BoxCollider2D platform in platforms)
        {
            platform.enabled = true;
        }
    }
}

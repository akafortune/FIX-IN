using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : SpecialTile
{
    GameObject greenGuy;
    GreenGuy ggScript;
    Rigidbody2D greenGuyRB;
    BoxCollider2D[] platforms;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        greenGuy = GameObject.Find("GreenGuy");
        greenGuyRB = greenGuy.GetComponent<Rigidbody2D>();
        platforms = GameObject.Find("Platforms").GetComponentsInChildren<BoxCollider2D>();
        ggScript = greenGuy.GetComponent<GreenGuy>();
    }

    protected override void Update()
    {
        if (!spriteRenderer.enabled)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        GreenGuy.SetBounce(false);
    }

    protected override void doAction()
    {
        GreenGuy.SetBounce(true);
    }

    protected override void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("SpecialBrickTrigger"))
        {
            doAction();
        }
    }

    protected override void stopAction()
    {

    }
}

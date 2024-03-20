using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : SpecialTile
{
    protected Transform otherTeleporter;
    GameObject greenGuy;
    BoxCollider2D[] platforms;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        greenGuy = GameObject.Find("GreenGuy");
        effectLength = 5;
        assignTeleporter();
        platforms = GameObject.Find("Platforms").GetComponentsInChildren<BoxCollider2D>();
}

    protected override void doAction()
    {
        if(otherTeleporter == null)
        {
            assignTeleporter();
        }
        foreach (BoxCollider2D boxCollider in platforms)
        {
            boxCollider.enabled = true;
        }
        otherTeleporter.GetComponent<Teleporter>().disableTeleporter();
        greenGuy.transform.position = new Vector2(otherTeleporter.position.x,otherTeleporter.position.y + .5f);
        this.transform.GetChild(0).gameObject.SetActive(false);
        otherTeleporter.GetChild(0).gameObject.SetActive(false);
    }

    protected override void stopAction()
    {
        this.transform.GetChild(0).gameObject.SetActive(true);
        otherTeleporter.GetChild(0).gameObject.SetActive(true);
        otherTeleporter.GetComponent<Teleporter>().enableTeleporter();
    }

    private void assignTeleporter()
    {
        Teleporter[] teleporters = GameObject.FindObjectsByType<Teleporter>(FindObjectsSortMode.None);
        foreach(Teleporter teleporter in teleporters)
        {
            if(teleporter.otherTeleporter == null && teleporter.transform != this.transform)
            {
                otherTeleporter = teleporter.transform;
                teleporter.otherTeleporter = this.transform;
                break;
            }
        }
    }

    protected override void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("SpecialBrickTrigger") && !effectActive)
        {
            timeStart = Time.time;
            doAction();
            effectActive = true;
        }
    }

    protected void enableTeleporter()
    {
        effectActive = false;
    }    

    protected void disableTeleporter()
    {
        effectActive = true;
        timeStart = Time.time;
    }
}

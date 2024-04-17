using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Teleporter : SpecialTile
{
    protected Transform otherTeleporter;
    GameObject greenGuy;
    BoxCollider2D[] platforms;
    public GameObject[] portalParticles;
     ParticleSystem[] ps;
    protected ParticleSystem[] otherTeleporterPS;
    public static int brokenPortals;

    // Start is called before the first frame update
    protected override void Start()
    {
        ps = GetComponentsInChildren<ParticleSystem>();
        base.Start();
        greenGuy = GameObject.Find("GreenGuy");
        effectLength = 5;
        assignTeleporter();
        platforms = GameObject.Find("Platforms").GetComponentsInChildren<BoxCollider2D>();
        brokenPortals = 0;
    }

    protected override void Update()
    {
        rb.AddForce(Vector2.zero);
        if (Time.time > timeStart + effectLength && effectActive)
        {
            stopAction();
            effectActive = false;
        }

        if (!spriteRenderer.enabled && ps[0].particleCount == 0)
        {
            Destroy(gameObject);
        }

        if (!effectActive && otherTeleporter != null && !broken)
        {
            foreach (ParticleSystem p in ps)
            {
                var e = p.emission;
                e.rateOverTime = 1000f;
            }
        }
        else
        {
            foreach (ParticleSystem p in ps)
            {
                var e = p.emission;
                e.rateOverTime = 0;
            }
        }
    }

    protected override void doAction()
    {
        if(otherTeleporter == null)
        {
            Debug.Log("DoAction Assign");
            assignTeleporter();
            return;
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
        if (otherTeleporter != null)
        {
            this.transform.GetChild(0).gameObject.SetActive(true);
            otherTeleporter.GetChild(0).gameObject.SetActive(true);
            otherTeleporter.GetComponent<Teleporter>().enableTeleporter();
        }
    }

    private void assignTeleporter()
    {
        Teleporter[] teleporters = GameObject.FindObjectsByType<Teleporter>(FindObjectsSortMode.None);
        foreach(Teleporter teleporter in teleporters)
        {
            if(teleporter.otherTeleporter == null && teleporter.transform != this.transform && !teleporter.broken)
            {
                otherTeleporter = teleporter.transform;
                otherTeleporterPS = otherTeleporter.GetComponentsInChildren<ParticleSystem>();
                teleporter.otherTeleporter = this.transform;
                teleporter.otherTeleporterPS = this.GetComponentsInChildren<ParticleSystem>();
                effectActive = otherTeleporter.GetComponent<Teleporter>().effectActive;
                foreach (ParticleSystem p in ps)
                {
                    var e = p.emission;
                    e.rateOverTime = 1000f;
                }

                foreach (ParticleSystem p in otherTeleporterPS)
                {
                    var e = p.emission;
                    e.rateOverTime = 1000f;
                }

                break;
            }
        }
    }

    protected override void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("SpecialBrickTrigger") && !effectActive && !broken && otherTeleporter != null)
        {
            timeStart = Time.time;
            doAction();
            effectActive = true;
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Ball"))
        {
            brokenPortals++;
            foreach (ParticleSystem p in ps)
            {
                var e = p.emission;
                e.rateOverTime = 0f;
            }
            broken = true;
            ReAssign();
            base.OnCollisionEnter2D(collision);
        }
    }

    protected void enableTeleporter()
    {
        effectActive = false;
        foreach (ParticleSystem p in ps)
        {
            var e = p.emission;
            e.rateOverTime = 1000f;
        }

        foreach (ParticleSystem p in otherTeleporterPS)
        {
            var e = p.emission;
            e.rateOverTime = 1000f;
        }
    }    

    protected void disableTeleporter()
    {
        Debug.Log("Deactive");
        effectActive = true;
        timeStart = Time.time;
        foreach (ParticleSystem p in ps)
        {
            var e = p.emission;
            e.rateOverTime = 0f;
        }

        foreach (ParticleSystem p in otherTeleporterPS)
        {
            var e = p.emission;
            e.rateOverTime = 0f;
        }
    }

    protected override void cancelBrick()
    {
        foreach (ParticleSystem p in ps)
        {
            var e = p.emission;
            e.rateOverTime = 0f;
        }
        broken = true;
        ReAssign();
        base.cancelBrick();
    }

    public override void RemoveBrick()
    {
        foreach (ParticleSystem p in ps)
        {
            var e = p.emission;
            e.rateOverTime = 0f;
        }
        broken = true;
        base.RemoveBrick();
    }

    protected void ReAssign()
    {
        Debug.Log("Reassign");
        if (otherTeleporter != null)
        {
            Teleporter otherTeleporterScript = otherTeleporter.GetComponent<Teleporter>();
            otherTeleporterScript.otherTeleporter = null;
            otherTeleporterScript.assignTeleporter();
            if (otherTeleporterScript.otherTeleporter == null)
            {
                foreach (ParticleSystem p in otherTeleporterPS)
                {
                    var e = p.emission;
                    e.rateOverTime = 0f;
                }
            }
        }
    }
}

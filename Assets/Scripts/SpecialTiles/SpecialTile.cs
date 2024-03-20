using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialTile : MonoBehaviour
{
    protected bool effectActive;
    protected float timeStart;
    protected float effectLength;

    // Start is called before the first frame update
    void Start()
    {
        effectActive = false;
    }

    protected virtual void Update()
    {
        if (Time.time > timeStart + effectLength && effectActive)
        {
            stopAction();
            effectActive = false;
        }
    }

    // Update is called once per frame
    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("SpecialBrickTrigger") && !effectActive)
        {
            timeStart = Time.time;
            doAction();
            effectActive=true;
        }
    }

    protected virtual void doAction() { }
    protected virtual void stopAction() { }
}

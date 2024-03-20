using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : SpecialTile
{
    float cooldownStart;
    float cooldownLength;
    GameObject shield;
    // Start is called before the first frame update
    void Start()
    {
        shield = transform.GetChild(0).gameObject;
        effectLength = 10;
        cooldownLength = 10;
        cooldownStart = -10;
    }
    protected override void OnTriggerStay2D(Collider2D collision)
    {
        if(cooldownStart + cooldownLength < Time.time)
        {
            base.OnTriggerStay2D(collision);
        }
    }
    protected override void doAction()
    {
        shield.SetActive(true);
    }

    protected override void stopAction()
    {
        shield.SetActive(false);
        cooldownStart = Time.time;
    }
}

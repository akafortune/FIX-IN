using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : SpecialTile
{
    float cooldownStart;
    float cooldownLength;
    GameObject shield;

    public Animator animator;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        shield = transform.GetChild(0).gameObject;
        effectLength = 10;
        cooldownLength = 10;
        cooldownStart = -10;

        animator = GameObject.Find("Sheild Particles").transform.GetChild(0).gameObject.GetComponent<Animator>();
        animator.SetBool("Stop", true);
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
        animator.SetBool("Stop", false);
        animator.SetTrigger("Start");
    }

    protected override void stopAction()
    {
        shield.SetActive(false);
        cooldownStart = Time.time;
        animator.SetBool("Stop", true);
    }

    void FixedUpdate()
    {
        animator.ResetTrigger("Start");
    }
}

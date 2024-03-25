using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : SpecialTile
{
    float cooldownStart;
    float cooldownLength;
    GameObject shield;
    GameObject shieldParticles;
    int hits;

    public Animator animator;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        shield = transform.GetChild(0).gameObject;
        shieldParticles = transform.GetChild(1).transform.GetChild(0).gameObject;
        effectLength = 10;
        cooldownLength = 10;
        cooldownStart = -10;

        animator = shieldParticles.GetComponent<Animator>();
        animator.SetBool("Stop", true);
    }
    protected override void OnTriggerStay2D(Collider2D collision)
    {
        if(cooldownStart + cooldownLength < Time.time && BaseBuilding.GameMode != BaseBuilding.Mode.build)
        {
            base.OnTriggerStay2D(collision);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Ball"))
        {
            hits++;
            if(hits == 4)
            {
                stopAction();
                effectActive = false;
            }
            IEnumerator Routine = decreaseSizeRoutine(1-(hits*.1f));
            StartCoroutine(Routine);
        }
    }
    protected override void doAction()
    {
        shield.transform.localScale = (Vector3.one);
        shieldParticles.transform.localScale = (Vector3.one);
        shield.SetActive(true);
        animator.SetBool("Stop", false);
        animator.SetTrigger("Start");
        hits = 0;
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

    IEnumerator decreaseSizeRoutine(float size)
    {
        while (shield.transform.localScale.x > size)
        {
            Debug.Log("Doing the thing");
            shield.transform.localScale = (shield.transform.localScale *= .995f);
            shieldParticles.transform.localScale = (shieldParticles.transform.localScale *= .995f);
            yield return new WaitForFixedUpdate();
        }
        yield return null;

    }
}

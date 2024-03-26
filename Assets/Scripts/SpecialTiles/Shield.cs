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

    public Animator ShieldAnimator;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        shield = transform.GetChild(0).gameObject;
        shieldParticles = transform.GetChild(1).transform.GetChild(0).gameObject;
        effectLength = 10;
        cooldownLength = 10;
        cooldownStart = -10;

        ShieldAnimator = shieldParticles.GetComponent<Animator>();
        ShieldAnimator.SetBool("Stop", true);
        shield.SetActive(false);
    }
    protected override void OnTriggerStay2D(Collider2D collision)
    {
        if(cooldownStart + cooldownLength < Time.time && BaseBuilding.GameMode != BaseBuilding.Mode.build)
        {
            base.OnTriggerStay2D(collision);
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Ball"))
        {
            if (shield.activeInHierarchy)
            {
                hits++;
                if (hits == 4)
                {
                    stopAction();
                    effectActive = false;
                }
                IEnumerator Routine = decreaseSizeRoutine(1 - (hits * .1f));
                StartCoroutine(Routine);
            }
            else
                base.OnCollisionEnter2D(collision);
        }
    }
    protected override void doAction()
    {
        shield.transform.localScale = (Vector3.one);
        shieldParticles.transform.localScale = (Vector3.one);
        shield.SetActive(true);
        ShieldAnimator.SetBool("Stop", false);
        ShieldAnimator.SetTrigger("Start");
        hits = 0;
    }

    protected override void stopAction()
    {
        shield.SetActive(false);
        cooldownStart = Time.time;
        ShieldAnimator.SetBool("Stop", true);
    }

    void FixedUpdate()
    {
        ShieldAnimator.ResetTrigger("Start");
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

    protected override void cancelBrick()
    {
        stopAction();
        base.cancelBrick();
    }
}

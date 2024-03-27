using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : SpecialTile
{
    float cooldownStart;
    float cooldownLength;
    GameObject shield;
    GameObject shieldParticles;
    public Animator lever;
    int hits;

    public Animator ShieldAnimator;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        shield = transform.GetChild(0).gameObject;
        shieldParticles = transform.GetChild(1).transform.GetChild(0).gameObject;
        effectLength = 10;
        cooldownLength = 15;
        cooldownStart = -10;

        ShieldAnimator = shieldParticles.GetComponent<Animator>();
        ShieldAnimator.SetBool("Stop", true);
        shield.SetActive(false);
        lever = transform.GetChild(2).GetComponent<Animator>();
    }
    protected override void OnTriggerStay2D(Collider2D collision)
    {
        //do nothing
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
        IEnumerator coroutine = resetShield();
        StartCoroutine(coroutine);
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

    public bool CanStart()
    {
        if (cooldownStart + cooldownLength < Time.time && !effectActive)
            return true;
        else
            return false;
    }

    public void StartShield()
    {
        timeStart = Time.time;
        IEnumerator coroutine = waitToStartShield();
        StartCoroutine(coroutine);
    }

    protected IEnumerator waitToStartShield()
    {
        yield return new WaitForSeconds(.5f);
        doAction();
        lever.SetBool("Off", true);
        effectActive = true;
    }

    protected IEnumerator resetShield()
    {
        yield return new WaitForSeconds(cooldownLength/4f);
        lever.SetInteger("WarmupStage", 1);
        yield return new WaitForSeconds(cooldownLength/4f);
        lever.SetInteger("WarmupStage", 2);
        yield return new WaitForSeconds(cooldownLength/4f);
        lever.SetInteger("WarmupStage", 3);
        yield return new WaitForSeconds(cooldownLength/4f);
        lever.SetBool("Off", false);
    }
}

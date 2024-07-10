using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Mine : SpecialTile
{
    bool canMine;
    SpriteRenderer mineIcon;
    ParticleSystem ps;
    public new void Start()
    {
        base.Start();
        canMine = true;
        mineIcon = transform.GetChild(0).GetComponent<SpriteRenderer>();
        ps = gameObject.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>();
    }
    public new void doAction()
    {
        RoundManager.resources += 1;
        canMine = false;
        ps.Play();
        IEnumerator coroutine = waitToMine();
        StartCoroutine(coroutine);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        mineIcon.enabled = false;
    }
    public void ShowMine()
    {
        if(canMine)
            mineIcon.enabled = true;
    }
    public bool getMineable()
    {
        return canMine;
    }

    protected IEnumerator waitToMine()
    {
        yield return new WaitForSeconds(1.5f);
        canMine = true;
    }

    protected override void cancelBrick()
    {
        base.cancelBrick();
        associatedBrick.removeSpecialBrick();
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if(collision.gameObject.name.Equals("Ball"))
        {
            associatedBrick.removeSpecialBrick();
        }
    }
}

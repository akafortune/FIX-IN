using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : SpecialTile
{
    bool canMine;
    SpriteRenderer mineIcon;
    public new void Start()
    {
        base.Start();
        canMine = true;
        mineIcon = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }
    public new void doAction()
    {
        BaseBuilding.resources += 1;
        canMine = false;
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
        yield return new WaitForSeconds(1);
        canMine = true;
    }

    protected override void cancelBrick()
    {
        base.cancelBrick();
        Destroy(this.gameObject);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if(collision.gameObject.name.Equals("Ball"))
        {
            Destroy(this.gameObject);
        }
    }
}

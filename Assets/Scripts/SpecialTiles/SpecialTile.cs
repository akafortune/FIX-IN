using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialTile : MonoBehaviour
{
    protected bool effectActive;
    protected float timeStart;
    protected float effectLength;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        effectActive = false;
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        rb.AddForce(Vector2.zero);
        if (Time.time > timeStart + effectLength && effectActive)
        {
            stopAction();
            effectActive = false;
        }
    }

    // Update is called once per frame
    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("SpecialBrickTrigger") && !effectActive && GreenGuy.canJump)
        {
            timeStart = Time.time;
            doAction();
            effectActive=true;
        }
    }

    protected virtual void doAction() { }
    protected virtual void stopAction() { }
}

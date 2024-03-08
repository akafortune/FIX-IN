using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingSprite : MonoBehaviour
{
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(rb.velocity.x > 0)
        {
            transform.Rotate(0, 0, -10);
        }
        else if(rb.velocity.x < 0)
        {
            transform.Rotate(0, 0, 10);
        }
    }
}

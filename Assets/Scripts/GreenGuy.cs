using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenGuy : MonoBehaviour
{
    PlatformEffector2D[] platforms;
    Rigidbody2D rb;
    public int jumpForce;
    public float leftAndRight;
    // Start is called before the first frame update
    void Start()
    {
        platforms = GameObject.Find("Platforms").GetComponentsInChildren<PlatformEffector2D>();
        rb = GetComponent<Rigidbody2D>();
        jumpForce = 250;
        leftAndRight = 2;
    }

    // Update is called once per frame
    void Update()
    {
        float LR = leftAndRight * Time.deltaTime;
        //Debug.Log(rb.velocity.y);
        if(Input.GetKeyDown(KeyCode.S))
        {
            foreach (PlatformEffector2D plat in platforms)
            {
                plat.rotationalOffset = 180;
            }
        }
        if(Input.GetKeyUp(KeyCode.S))
        {
            foreach (PlatformEffector2D plat in platforms)
            {
                plat.rotationalOffset = 0;
            }
        }
        if(Input.GetKey(KeyCode.A))
        {
            transform.Translate(-LR, 0, 0, Space.World);
        }
        if(Input.GetKey(KeyCode.D))
        {
            transform.Translate(LR, 0, 0, Space.World);
        }    
        if (Input.GetKeyDown(KeyCode.W) && rb.velocity.y < 1 && rb.velocity.y > -1)
        {
            Debug.Log("jump");
            rb.AddForce(new Vector2(0, jumpForce));
        }
    }
}

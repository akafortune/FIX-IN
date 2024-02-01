using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenGuy : MonoBehaviour
{
    PlatformEffector2D[] platforms;
    Rigidbody2D rb;
    public int jumpForce;
    public float leftAndRight, stunClock, stunTime = 3;
    bool canJump = false, canMove = true;
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
        if(canMove)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                foreach (PlatformEffector2D plat in platforms)
                {
                    plat.rotationalOffset = 180;
                }
                canJump = false;
            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                foreach (PlatformEffector2D plat in platforms)
                {
                    plat.rotationalOffset = 0;
                    canJump = true;
                }
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(-LR, 0, 0, Space.World);
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(LR, 0, 0, Space.World);
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            if (Input.GetKeyDown(KeyCode.W) && canJump && rb.velocity.y < .25 && !Input.GetKey(KeyCode.S))
            {
                //Debug.Log("jump");
                rb.AddForce(new Vector2(0, jumpForce));
                canJump = false;
            }
            canJump = true;
        }
        if (canMove == false)
        {
            stunClock += Time.deltaTime;
        }

        if (stunClock > stunTime)
        {
            canMove = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 7) //platforms
        {
            canJump = true;
        }

        if(collision.gameObject.layer == 6) //ball
        {
            if(canMove)
            {
                canMove = false;
                stunClock = 0;
            }
        }
    }
}

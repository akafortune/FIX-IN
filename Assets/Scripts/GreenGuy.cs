using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenGuy : MonoBehaviour
{
    Animator animator;
    PlatformEffector2D[] platforms;
    Rigidbody2D rb;
    RaycastHit2D fixRay;
    public LayerMask layersToHit;
    public Transform rayOrigin;
    
    public int jumpForce;
    public int fixMod = 1;
    public float leftAndRight, stunClock, distance, stunTime = 3;
    bool canJump = false, canMove = true;
    // Start is called before the first frame update
    void Start()
    {
        platforms = GameObject.Find("Platforms").GetComponentsInChildren<PlatformEffector2D>();
        Physics2D.queriesHitTriggers = true; //making it so that ray can detect triggers
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        jumpForce = 250;
        leftAndRight = 2;
    }

    // Update is called once per frame
    void Update()
    {
        fixRay = Physics2D.Raycast(rayOrigin.position, new Vector2(fixMod , -1), distance, layersToHit);

        Debug.DrawLine(rayOrigin.position, rayOrigin.position + new Vector3(fixMod * distance, -1 * distance)); //visualising ray in editor

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
                fixMod = -1;
                transform.Translate(-LR, 0, 0, Space.World);
                transform.rotation = Quaternion.Euler(0, 180, 0);
                animator.SetBool("Walking", true);
            }
            if (Input.GetKey(KeyCode.D))
            {
                fixMod = 1;
                transform.Translate(LR, 0, 0, Space.World);
                transform.rotation = Quaternion.Euler(0, 0, 0);
                animator.SetBool("Walking", true);
            }
            if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            {
                animator.SetBool("Walking", false);
            }
            if (Input.GetKeyDown(KeyCode.W) && canJump && rb.velocity.y < .25 && !Input.GetKey(KeyCode.S))
            {
                //Debug.Log("jump");
                rb.AddForce(new Vector2(0, jumpForce));
                canJump = false;
                animator.SetTrigger("Jump");
            }

            if (Input.GetKeyDown(KeyCode.Space) && canJump)
            {
                if (fixRay.collider != null)
                {
                    checkCollider();
                }
            }
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
            animator.SetTrigger("Grounded");
        }

        if(collision.gameObject.layer == 6) //ball
        {
            if(canMove)
            {
                canMove = false;
                stunClock = 0;
                animator.SetTrigger("Stun");
            }
        }
    }

    public void checkCollider()
    {
        if (fixRay.collider.isTrigger)
        {
            fixRay.collider.gameObject.SendMessage("fixBrick");
            animator.SetTrigger("Fix");
            canMove = false;
            stunClock = 0;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static GreenGuy;
using static UnityEngine.UI.ContentSizeFitter;

public class FloorCheck : MonoBehaviour
{
    public GreenGuy greenGuy;
    public BoxCollider2D checkBox;
    public LayerMask platforms;
    private Rigidbody2D rb;
    public Animator animator;

    float clock = 0.2f;
    float timer = 0.0f;

    public bool HasJumped;

    // Start is called before the first frame update
    void Start()
    {
        HasJumped = true;
        animator = gameObject.GetComponentInParent<Animator>();
        rb = gameObject.GetComponentInParent<Rigidbody2D>();
    }

    public void JumpAnimCrt()
    {
        HasJumped = true;
        timer = 0.0f;
    }

    void Update()
    {
        RaycastHit2D PlatformDropCast = Physics2D.Raycast(gameObject.transform.position, new Vector2(0, -1), 0.3f, platforms);
        Debug.DrawLine(transform.position, transform.position + new Vector3(0, -.3f), Color.green);
        if (PlatformDropCast.collider != null && (Input.GetKeyDown(KeyCode.S) || Input.GetAxis("Vertical") < -.1f) && PlatformDropCast.collider.name != "PlatformLower" && Time.timeScale != 0 && canMove)
        {
            PlatformDropCast.collider.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
        if (HasJumped)
        {
            if (timer >= clock)
            {
                RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position, new Vector2(0 , -1), 0.05f, platforms);
                Debug.DrawLine(transform.position, transform.position + new Vector3(0,-.05f));
                if (hit.collider != null && rb.velocity.y < .25 && rb.velocity.y > -.25)
                {
                    animator.ResetTrigger("Jump");
                    GreenGuy.canJump = true;
                    greenGuy.animator.SetTrigger("Grounded");
                    HasJumped = false;
                    //animator.Play("Idle - Blink");
                }
            }
            else {timer += Time.deltaTime;}
        }
    }
    void FixedUpdate() { greenGuy.animator.ResetTrigger("Grounded");}







}

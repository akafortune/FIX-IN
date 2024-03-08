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

    public Animator animator;

    float clock = 0.2f;
    float timer = 0.0f;

    public bool HasJumped;

    // Start is called before the first frame update
    void Start()
    {
        HasJumped = true;
        animator = gameObject.GetComponentInParent<Animator>();
    }

    public void JumpAnimCrt()
    {
        HasJumped = true;
        timer = 0.0f;
    }

    void Update()
    {
        if (HasJumped)
        {
            if (timer >= clock)
            {
                RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position, new Vector2(0 , -1), 0.05f, platforms);
                Debug.DrawLine(transform.position, transform.position + new Vector3(0,-.05f));
                if (hit.collider != null)
                {
                    greenGuy.canJump = true;
                    greenGuy.animator.SetTrigger("Grounded");
                    HasJumped = false;

                }
            }
            else {timer += Time.deltaTime;}
        }
    }
    void FixedUpdate() { greenGuy.animator.ResetTrigger("Grounded");}







}

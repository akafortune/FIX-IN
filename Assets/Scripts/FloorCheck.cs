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

    float timer = 0.15f;
    float clock;

    bool jumpStart = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (jumpStart)
        {
            clock += Time.deltaTime;
        }
        RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position, new Vector2(0 , -1), 0.15f, platforms);
        Debug.DrawLine(transform.position, transform.position + new Vector3(0,-.15f));
        if (hit.collider != null)
        {
            greenGuy.canJump = true;
            if (clock > timer)
            {
                greenGuy.animator.SetTrigger("Grounded");
                clock = 0;
                jumpStart = false;
            }
        }
    }

    public void JumpAnimCrt()
    {
        jumpStart = true;
    }




}

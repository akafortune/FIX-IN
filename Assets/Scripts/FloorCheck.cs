using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GreenGuy;

public class FloorCheck : MonoBehaviour
{
    public GreenGuy greenGuy;
    public BoxCollider2D checkBox;

    public Animator animator;
    // Start is called before the first frame update

    bool flipSwitch;
    float offsetTimer;
    void Start()
    {
        animator = gameObject.GetComponentInParent<Animator>();
        flipSwitch = true;
        offsetTimer = .2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (flipSwitch == false && offsetTimer > 0)
        {
            offsetTimer -= Time.deltaTime;
        }
        //manage CheckBox
        if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Jump" && flipSwitch)
        {
            Debug.Log("UwU");
            checkBox.offset= new Vector2(0.0f, 0.44f);
            flipSwitch = false;
        }
        else if (offsetTimer <= 0)
        {
            Debug.Log("OwO");
            checkBox.offset= new Vector2(0.0f, 0.19f);
        }
    }

        private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 7) //platforms
        {
            greenGuy.canJump = true;
            greenGuy.animator.SetTrigger("Grounded");
            flipSwitch = true;
            offsetTimer = .2f;
        }
    }




}

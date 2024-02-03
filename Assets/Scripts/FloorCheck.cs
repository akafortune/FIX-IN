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
    void Start()
    {
        animator = gameObject.GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(animator.name);
        //wtf, it both does and does not exist ???? null refrence and outputs correctly. I have no words
        Debug.Log(animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);


        //manage CheckBox
        if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Jump")
        {
            checkBox.offset.Set(0.0f, 0.44f);
        }
        else
        {
            checkBox.offset.Set(0.0f, 0.0f);
        }
    }

        private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 7) //platforms
        {
            greenGuy.canJump = true;
            greenGuy.animator.SetTrigger("Grounded");
        }
    }




}

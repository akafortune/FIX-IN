using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    Transform Ball;
    public Animator Anim;
    // Start is called before the first frame update
    void Start()
    {
        Ball = GameObject.Find("Ball").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Ball.position.x;
        x = Mathf.Clamp(x, -2.25f, 2.25f);
        transform.position = new Vector3(x, 4.25f, 0);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            Anim.SetTrigger("Hit");
        }
    }
}

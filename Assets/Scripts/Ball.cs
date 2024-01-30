using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour
{
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        int HorzForce = Random.Range(15, 45);
        Debug.Log(HorzForce);
        int StartingDirection = Random.Range(0, 2); // 0 for left, 1 for right
        Debug.Log(StartingDirection);
        HorzForce = StartingDirection == 0 ? HorzForce : HorzForce * -1; 
        rb.AddRelativeForce(new Vector2(HorzForce, 150), ForceMode2D.Force);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Wall"))
        {
            rb.AddRelativeForce(rb.velocity, ForceMode2D.Force);
            Debug.Log("wall hit");
        }
    }
}

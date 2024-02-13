using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour
{
    Rigidbody2D rb;
    public float speedMultiplier;
    float LastXVelocity;
    static float testAugment;
    public static bool newAugment = true;
    public GameObject gameOverMenu;
    public AudioSource audioSource;
    public AudioClip wallBounce, paddleBounce, ggBounce, bottomWallBounce;
    public TextMeshProUGUI TestVersion;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        TestVersion = GameObject.Find("TestVersionText").GetComponent<TextMeshProUGUI>();
        speedMultiplier = 1.5f;

        if (newAugment)
        {
            testAugment = Random.Range(1.0f, 1.5f);
            newAugment = false;
        }

        speedMultiplier *= testAugment;
        GreenGuy.stunTime /= testAugment;
        //GreenGuy.speedMod *= testAugment;
        TestVersion.text = "Test Version 1." + testAugment.ToString("0.00");

        int HorzForce = Random.Range(15, 45); // Randomizes angle of ball
        //Debug.Log(HorzForce);
        int StartingDirection = Random.Range(0, 2); // 0 for left, 1 for right
        //Debug.Log(StartingDirection);
        HorzForce = StartingDirection == 0 ? HorzForce : HorzForce * -1; 
        rb.AddRelativeForce(new Vector2(HorzForce*speedMultiplier, 150*speedMultiplier), ForceMode2D.Force);
        rb.velocity *= 10000f;
        //Debug.Log(rb.velocity.magnitude);
    }

    void FixedUpdate()
    {
        LastXVelocity = rb.velocity.x;
        //Debug.Log(rb.velocity.x);
        while(rb.velocity.magnitude > 3.5 * speedMultiplier) // Prevents ball from going apeshit and flying off the map
        {
            rb.velocity = rb.velocity * 0.99f;
        }
        //while(rb.velocity.magnitude > 2.0)
        //{
          //  rb.velocity = rb.velocity * 1.01f;
        //}
    }

    private void OnCollisionEnter2D(Collision2D collision) // all of these checks are to keep the ball from bouncing in straight lines
    {
        if (collision.gameObject.name.Equals("Wall9PatchRight"))
        {
            rb.AddRelativeForce(new Vector2(50, 0), ForceMode2D.Force);
            int VertForce = rb.velocity.y > 0 ? 50 : -50;
            rb.AddForce(new Vector2(0, VertForce * speedMultiplier));
            audioSource.clip = wallBounce;
            audioSource.Play();
            //Debug.Log("R wall hit");
        }
        else if (collision.gameObject.name.Equals("Wall9PatchLeft"))
        {
            rb.AddRelativeForce(new Vector2(-50, 0), ForceMode2D.Force);
            int VertForce = rb.velocity.y > 0 ? 50 : -50;
            rb.AddForce(new Vector2(0, VertForce * speedMultiplier));
            audioSource.clip = wallBounce;
            audioSource.Play();
            //Debug.Log("L wall hit");
        }
        else if (collision.gameObject.name.Equals("Paddle"))
        {
            if (LastXVelocity < 0.5f  && LastXVelocity > -0.5f)
            {
                //Debug.Log("Fixed X");
                int HorzForce = LastXVelocity > 0 ? -50 : 50;
                rb.AddRelativeForce(new Vector2(HorzForce * speedMultiplier, 0), ForceMode2D.Force);
                audioSource.clip = paddleBounce;
                audioSource.Play();
                Debug.Log("Paddle Sound Play");
            }
        }
        else if (collision.gameObject.name.Equals("Wall9PatchBottom"))
        {
            Time.timeScale = 0f;
            gameOverMenu.SetActive(true);
            audioSource.clip = bottomWallBounce;
            audioSource.Play();
        }
        else if (collision.gameObject.name.Equals("GreenGuy"))
        {
            rb.velocity *= 3;
            audioSource.clip = ggBounce;
            audioSource.Play();
        }
    }
}

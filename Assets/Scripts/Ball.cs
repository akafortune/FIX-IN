using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour
{
    Rigidbody2D rb;
    public float speedMultiplier;
    float LastXVelocity;
    static float testAugment;
    private float mapSizeAugment;
    public static bool newAugment = true;
    public GameObject gameOverMenu;
    public AudioSource audioSource;
    public AudioClip wallBounce, paddleBounce, ggBounce, bottomWallBounce;
    public TextMeshProUGUI TestVersion;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //TestVersion = GameObject.Find("TestVersionText").GetComponent<TextMeshProUGUI>();
        speedMultiplier = 1.5f;
        int minAngle;
        int maxAngle;
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(3) || SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(4))
        {
            minAngle = 25;
            maxAngle = 65;
            mapSizeAugment = 1.5f;
        }
        else
        {
            mapSizeAugment = 1;
            minAngle = 15;
            maxAngle = 45;
        }

        if (newAugment)
        {
            testAugment = Random.Range(1.0f, 1.5f);
            newAugment = false;
        }

        speedMultiplier *= testAugment;
        GreenGuy.stunTime /= testAugment;
        //GreenGuy.speedMod *= testAugment;
        //TestVersion.text = "Test Version 1." + testAugment.ToString("0.00");

        int HorzForce = Random.Range(minAngle, maxAngle); // Randomizes angle of ball
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
            if (LastXVelocity < 0.5f * mapSizeAugment  && LastXVelocity > -0.5f * mapSizeAugment)
            {
                //Debug.Log("Fixed X");
                int HorzForce = LastXVelocity > 0 ? -50 : 50;
                rb.AddRelativeForce(new Vector2(HorzForce * speedMultiplier * mapSizeAugment, 0), ForceMode2D.Force);
            }
            audioSource.PlayOneShot(paddleBounce);
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

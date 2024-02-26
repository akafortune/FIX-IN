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
    public float StaticspeedMultiplier;

    public float RampspeedMultiplier; //multiplier applies to static

    public float FinalspeedMultiplier;
    float LastXVelocity;
    public float augment;
    private float mapSizeAugment;
    public GameObject gameOverMenu;
    public AudioSource audioSource;
    public AudioClip wallBounce, paddleBounce, ggBounce, bottomWallBounce;
    public TextMeshProUGUI TestVersion;

    public GameObject pauseMenu;
    public float gameTimer;

    // Start is called before the first frame update
    void Awake()
    {
        StaticspeedMultiplier = 1.5f;
        RampspeedMultiplier = 1f;
        augment = 1.3f;

        gameTimer = 0f;
        rb = GetComponent<Rigidbody2D>();
        //TestVersion = GameObject.Find("TestVersionText").GetComponent<TextMeshProUGUI>();
        //pauseMenu = GameObject.Find("PauseMenu");
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

        StaticspeedMultiplier *= augment;
        GreenGuy.stunTime /= augment;
        FinalspeedMultiplier = StaticspeedMultiplier;
        //GreenGuy.speedMod *= augment;
        //TestVersion.text = "Test Version 1." + augment.ToString("0.00");

        int HorzForce = Random.Range(minAngle, maxAngle); // Randomizes angle of ball
        //int HorzForce = 0; // Sets angle of ball to 0
        //Debug.Log(HorzForce);
        int StartingDirection = Random.Range(0, 2); // 0 for left, 1 for right
        //Debug.Log(StartingDirection);
        HorzForce = StartingDirection == 0 ? HorzForce : HorzForce * -1; 
        rb.AddRelativeForce(new Vector2(HorzForce*FinalspeedMultiplier, 150*FinalspeedMultiplier), ForceMode2D.Force);
        rb.velocity *= 10000f;
        //Debug.Log(rb.velocity.magnitude);
    }

    void FixedUpdate()
    {
        LastXVelocity = rb.velocity.x;
        //Debug.Log(rb.velocity.x);
        while(rb.velocity.magnitude > 3.5 * FinalspeedMultiplier) // Prevents ball from going apeshit and flying off the map
        {
            rb.velocity = rb.velocity * 0.99f;
        }
        //while(rb.velocity.magnitude > 2.0)
        //{
          //  rb.velocity = rb.velocity * 1.01f;
        //}

        //Speed Ramping
        if (pauseMenu.activeSelf == false)
        {
            gameTimer += Time.fixedDeltaTime;
            RampSpeed();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) // all of these checks are to keep the ball from bouncing in straight lines
    {
        if (collision.gameObject.name.Equals("Wall9PatchRight"))
        {
            rb.AddRelativeForce(new Vector2(50, 0), ForceMode2D.Force);
            int VertForce = rb.velocity.y > 0 ? 50 : -50;
            rb.AddForce(new Vector2(0, VertForce * FinalspeedMultiplier));
            audioSource.clip = wallBounce;
            audioSource.Play();
            //Debug.Log("R wall hit");
        }
        else if (collision.gameObject.name.Equals("Wall9PatchLeft"))
        {
            rb.AddRelativeForce(new Vector2(-50, 0), ForceMode2D.Force);
            int VertForce = rb.velocity.y > 0 ? 50 : -50;
            rb.AddForce(new Vector2(0, VertForce * FinalspeedMultiplier));
            audioSource.clip = wallBounce;
            audioSource.Play();
            //Debug.Log("L wall hit");
        }
        else if (collision.gameObject.name.Equals("Triangle"))
        {
            audioSource.PlayOneShot(wallBounce);
        }
        else if (collision.gameObject.name.Equals("Paddle"))
        {
            if (LastXVelocity < 0.5f * mapSizeAugment  && LastXVelocity > -0.5f * mapSizeAugment)
            {
                //Debug.Log("Fixed X");
                int HorzForce = LastXVelocity > 0 ? -50 : 50;
                rb.AddRelativeForce(new Vector2(HorzForce * FinalspeedMultiplier * mapSizeAugment, 0), ForceMode2D.Force);
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

    void RampSpeed()
    {
        if (gameTimer > 30)
        {
            float y = gameTimer-30f;
            float x = (-1f)*(25f/(Mathf.Pow(y, 2f)+(186f*y)+86.5f)) + (1.3f);
            if (x < 1)
            {
                RampspeedMultiplier = 1;
            }
            else
            {
                RampspeedMultiplier = x;
            }
            FinalspeedMultiplier = StaticspeedMultiplier*RampspeedMultiplier;
            Debug.Log(GreenGuy.stunTime);
            GreenGuy.stunTime = 1.7f / (augment + (RampspeedMultiplier/3f));
        }
    }
}

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
    private float ctdTimer;
    private bool countingDown;
    private bool[] countDownAudioPlayed;
    private TextMeshProUGUI countdownText;
    public float RampspeedMultiplier; //multiplier applies to static

    public float FinalspeedMultiplier;
    float LastXVelocity;
    public float augment;
    private float mapSizeAugment;
    public GameObject gameOverMenu;
    private GameObject arrow;
    public AudioSource audioSource;
    public AudioClip wallBounce, paddleBounce, ggBounce, bottomWallBounce, countDownSound, launchSound, ballExplode;
    public TextMeshProUGUI TestVersion;

    private int minAngle, maxAngle, ballAngle;

    public GameObject pauseMenu;
    public float gameTimer;
    private Vector3 spawnPos;

    private int bricksBroken;

    private TrailRenderer trail;
    public static int hits;
    private GameObject explodingParticle;
    private Vector3 particleLocation;

    // Start is called before the first frame update
    void Awake()
    {
        countDownAudioPlayed = new bool[3];
        StaticspeedMultiplier = 1.25f;
        RampspeedMultiplier = 1f;
        augment = 1.3f;
        spawnPos = transform.position;
        particleLocation = transform.position;
        gameTimer = 0f;
        rb = GetComponent<Rigidbody2D>();
        explodingParticle = GameObject.Find("BallPop");
        explodingParticle.SetActive(false);
        arrow = GameObject.Find("Pointer");
        arrow.SetActive(false);
        trail = GameObject.Find("Trail").GetComponent<TrailRenderer>();
        countdownText = GameObject.Find("Countdown").GetComponent<TextMeshProUGUI>();
        countDownSound = (AudioClip) Resources.Load("SFX/Countdown");
        launchSound = (AudioClip) Resources.Load("SFX/Launch");
        //TestVersion = GameObject.Find("TestVersionText").GetComponent<TextMeshProUGUI>();
        //pauseMenu = GameObject.Find("PauseMenu");
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(3) || SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(4))
        {
            mapSizeAugment = 1.5f;
            minAngle = 25;
            maxAngle = 65;
        }
        else
        {
            mapSizeAugment = 1;
            minAngle = 15;
            maxAngle = 45;
        }
        ctdTimer = 3.0f;
        StaticspeedMultiplier *= augment;
        GreenGuy.stunTime /= augment;
        FinalspeedMultiplier = StaticspeedMultiplier;
    }

    public void LaunchSequence()
    {
        ResetBall();
        Rotate();
        countingDown = true;
        ctdTimer = 3.5f;
    } //Sets the following 4 methods in motion

    public void ResetBall()
    {
        countDownAudioPlayed = new bool[3];
        hits = 0;
        rb.velocity = Vector3.zero;
        gameObject.SetActive(true);
        transform.position = spawnPos;
        explodingParticle.transform.position = particleLocation;
        trail.Clear();
        transform.eulerAngles = new Vector3(0, 0, 180);
    } //Puts the ball back where it started

    public void Rotate()
    {
        ballAngle = Random.Range(minAngle, maxAngle + 1); // Randomizes angle of ball
        //int HorzForce = 0; // Sets angle of ball to 0
        //Debug.Log(HorzForce);
        int StartingDirection = Random.Range(0, 2); // 0 for left, 1 for right
        //Debug.Log(StartingDirection);
        ballAngle = StartingDirection == 0 ? ballAngle : ballAngle * -1;
        transform.Rotate(0, 0, ballAngle);
    } //Sets the ball to random angle

    void Countdown()
    {
        if (ctdTimer > 3)
        {
            //waiting a second to play audio
        }
        else if (ctdTimer > 2)
        {
            if (!countDownAudioPlayed[0])
            {
                arrow.SetActive(true);
                audioSource.PlayOneShot(countDownSound);
                countDownAudioPlayed[0] = true;
            }
            countdownText.text = "3";
        }
        else if (ctdTimer > 1)
        {
            if (!countDownAudioPlayed[1])
            {
                audioSource.PlayOneShot(countDownSound);
                countDownAudioPlayed[1] = true;
            }
            countdownText.text = "2";
        }
        else if (ctdTimer > 0)
        {
            if (!countDownAudioPlayed[2])
            {
                audioSource.PlayOneShot(countDownSound);
                countDownAudioPlayed[2] = true;
            }
            countdownText.text = "1";
        }
        else
        {
            countdownText.text = "";
            countingDown = false;
            Launch();
        }
    } //Begins launch countdown and activates indicator arrow

    public void Launch()
    {
        explodingParticle.transform.position = transform.position;
        explodingParticle.SetActive(false);
        BaseBuilding.lastBrickBuilt = true;
        //transform.Rotate(0, 0, -HorzForce);
        arrow.SetActive(false);
        //GreenGuy.speedMod *= augment;
        //TestVersion.text = "Test Version 1." + augment.ToString("0.00");
        rb.AddRelativeForce(new Vector2(0, 150 * FinalspeedMultiplier), ForceMode2D.Force);
        audioSource.PlayOneShot(launchSound);
        rb.velocity *= 10000f;
        //Debug.Log(rb.velocity.magnitude);
    } //Makes the ball start

    void FixedUpdate() //physics fuckery
    {
        LastXVelocity = rb.velocity.x;
        //Debug.Log(rb.velocity.x);
        while (rb.velocity.magnitude > 3.5 * FinalspeedMultiplier) // Prevents ball from going apeshit and flying off the map
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

    void Update() // Countdown and Trail color changes
    {
        if (countingDown)
        {
            ctdTimer -= Time.deltaTime;
            Countdown();
        }
        Color tailColor = trail.startColor;
        switch (hits)
        {
            case 0:
            case 1:
                tailColor = new Color(1, 1, 1);
                break;
            case 2:
                tailColor = new Color(255 / 255f, 244 / 255f, 146 / 255f);
                break;
            case 3:
                tailColor = new Color(255 / 255f, 116 / 255f, 37 / 255f);
                break;
            //case 4:
            //    tailColor = new Color();
            //    break;
            default:
                explodingParticle.SetActive(true);
                audioSource.PlayOneShot(ballExplode);
                particleLocation = transform.position;
                LaunchSequence();
                break;
        }
        trail.startColor = tailColor;
    }

    private void OnCollisionEnter2D(Collision2D collision) // all of these checks are to keep the ball from bouncing in straight lines
    {
        if (collision.gameObject.name.Equals("Wall9PatchRight"))
        {
            rb.AddRelativeForce(new Vector2(50, 0), ForceMode2D.Force);
            int VertForce = rb.velocity.y > 0 ? 50 : -50;
            rb.AddForce(new Vector2(0, VertForce * FinalspeedMultiplier));
            audioSource.PlayOneShot(wallBounce);
            //Debug.Log("R wall hit");
        }
        else if (collision.gameObject.name.Equals("Wall9PatchLeft"))
        {
            rb.AddRelativeForce(new Vector2(-50, 0), ForceMode2D.Force);
            int VertForce = rb.velocity.y > 0 ? 50 : -50;
            rb.AddForce(new Vector2(0, VertForce * FinalspeedMultiplier));
            audioSource.PlayOneShot(wallBounce);
            //Debug.Log("L wall hit");
        }
        else if (collision.gameObject.name.Equals("Triangle"))
        {
            audioSource.PlayOneShot(wallBounce);
        }
        else if (collision.gameObject.name.Equals("Paddle"))
        {
            hits = 0;
            if (LastXVelocity < 0.5f * mapSizeAugment && LastXVelocity > -0.5f * mapSizeAugment)
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
            audioSource.PlayOneShot(bottomWallBounce);
        }
        else if (collision.gameObject.name.Equals("GreenGuy"))
        {
            rb.velocity *= 100;
            audioSource.PlayOneShot(ggBounce);
        }
    }

    void RampSpeed()
    {
        if (gameTimer > 30)
        {
            float y = gameTimer - 30f;
            float x = (-1f) * (25f / (Mathf.Pow(y, 2f) + (186f * y) + 86.5f)) + (1.3f);
            if (x < 1)
            {
                RampspeedMultiplier = 1;
            }
            else
            {
                RampspeedMultiplier = x;
            }
            FinalspeedMultiplier = StaticspeedMultiplier * RampspeedMultiplier;
            //Debug.Log(GreenGuy.stunTime);
            GreenGuy.stunTime = 1.7f / (augment + (RampspeedMultiplier / 3f));
        }
    }

    public void Explode()
    {
        hits = 5;
    }
}

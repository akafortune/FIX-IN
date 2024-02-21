using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GreenGuy : MonoBehaviour
{
    public Animator animator; //
    public PlatformEffector2D[] platforms; //
    public Rigidbody2D rb; //
    public RaycastHit2D fixRay; //
    public LayerMask layersToHit;
    public Transform rayOrigin;

    public GameObject floorRay;
    
    public int jumpForce;
    public int fixMod = 1;
    public static float speedMod = 1;
    public float horizontalSpeed = 2f;
    public float stunClock, distance;
    public static float stunTime = 2.5f;
    public bool canJump = false, canMove = true; //

    public float buildTimer, buildClock = 0;

    public bool building = false, stunned = false;

    private float oneSecond = 1f;
    public float score;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;

    public float yOffset;
    public GameObject floatingText;
    public BoxCollider2D headBox;

    public BoxCollider2D checkBox;

    public AudioSource audioSource;
    public AudioClip walk, jump, brickFix; // haven't found a good walk sound yet
    // Start is called before the first frame update
    void Start()
    {
        platforms = GameObject.Find("Platforms").GetComponentsInChildren<PlatformEffector2D>();
        Physics2D.queriesHitTriggers = true; //making it so that ray can detect triggers
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        jumpForce = 235;
        horizontalSpeed = 2;
        buildTimer = 1.3f;
        stunTime = 1.7f;
        highScoreText.text = PlayerPrefs.GetInt("HighScore").ToString();
        canJump = true;
        //headBox = GameObject.Find("HeadBox").GetComponentInChildren<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        fixRay = Physics2D.Raycast(rayOrigin.position, new Vector2(fixMod , -1), distance, layersToHit);

        Debug.DrawLine(rayOrigin.position, rayOrigin.position + new Vector3(fixMod * distance, -1 * distance)); //visualising ray in editor

        float adjustedSpeed = horizontalSpeed * Time.deltaTime * speedMod;
        //Debug.Log(rb.velocity.y);
        if(canMove)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                foreach (PlatformEffector2D plat in platforms)
                {
                    plat.rotationalOffset = 180;
                }
            }
            if (Input.GetKey(KeyCode.A))
            {
                fixMod = -1;
                transform.Translate(-adjustedSpeed, 0, 0, Space.World);
                transform.rotation = Quaternion.Euler(0, 180, 0);
                animator.SetBool("Walking", true);
                audioSource.clip = walk;
                if(!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
            if (Input.GetKey(KeyCode.D))
            {
                fixMod = 1;
                transform.Translate(adjustedSpeed, 0, 0, Space.World);
                transform.rotation = Quaternion.Euler(0, 0, 0);
                animator.SetBool("Walking", true);
                audioSource.clip = walk;
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
            if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            {
                animator.SetBool("Walking", false);
            }
            if (Input.GetKeyDown(KeyCode.W) && canJump && rb.velocity.y < .25 && !Input.GetKey(KeyCode.S))
            {
                //Debug.Log("jump");
                rb.AddForce(new Vector2(0, jumpForce));
                canJump = false;
                animator.SetTrigger("Jump");
                audioSource.PlayOneShot(jump);
                floorRay.SendMessage("JumpAnimCrt");
                foreach(PlatformEffector2D platform in platforms)
                {
                    platform.rotationalOffset = 0;
                }
            }

            if (Input.GetKeyDown(KeyCode.Space) && canJump)
            {
                if (fixRay.collider != null)
                {
                    checkCollider();
                }
            }
        }

        if (stunned)
        {
            stunClock += Time.deltaTime;
        }

        if (stunClock > stunTime && stunned)
        {
            canMove = true;
            animator.SetBool("Stun", false);
            stunned = false;
        }

        if (building)
        {
            buildClock += Time.deltaTime;
        }

        if (buildClock > buildTimer && building)
        {
            canMove = true;
            animator.SetBool("Swinging", false);
            building = false;
            score += 10;
            // Trigger floating text here
            if(floatingText != null)
            { 
                ShowFloatingText("10");
            }
        }
        scoreText.text = ((int)score).ToString();
        if (score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", (int)score);
            highScoreText.text = PlayerPrefs.GetInt("HighScore").ToString();
        }

        if(fixRay.collider != null)
        {
            if (fixRay.collider.isTrigger)
            {
                fixRay.collider.SendMessage("ShowIndicator");
            }
        }
    }

    private void FixedUpdate()
    {
        score += oneSecond * Time.fixedDeltaTime;
        scoreText.text = ((int)score).ToString();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*if(collision.gameObject.layer == 7) //platforms
        {
            canJump = true;
            animator.SetTrigger("Grounded");
        }*/

        if(collision.gameObject.layer == 6) //ball
        {
                canMove = false;
                stunClock = 0;
                animator.SetBool("Stun", true);
                animator.SetTrigger("StunStart");
                stunned = true;
                if (building == true)
                {
                    BuidlingManagement("Stun");
                    building = false;
                }
        }
    }

    public void checkCollider()
    {
        if (fixRay.collider.isTrigger)
        {
            fixRay.collider.gameObject.SendMessage("fixBrick");
            animator.SetTrigger("Fix");
            animator.SetBool("Swinging", true);
            building = true;
            buildClock = 0;
            canMove = false;
            audioSource.PlayOneShot(brickFix);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            score += 5;
            ShowFloatingText("5");
        }
    }


    //Building Handler

    public void BuidlingManagement(string input)
    {
        if (input == "Stun" && building)
        {
            fixRay.collider.gameObject.SendMessage("cancelBrick");
        }
        if (input == "Build")
        {
            //For when we want to mess with the anim speed of building for the bricks
        }
    }

    public void ShowFloatingText(string points)
    {
        // text pop up should appear in the right direction no matter where the player faces
        GameObject flText = Instantiate(floatingText, transform.position + new Vector3(0, yOffset, 0), Quaternion.identity);
        flText.GetComponentInChildren<TextMesh>().text = "+" + points;
    }
}

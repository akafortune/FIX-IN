using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GreenGuy : MonoBehaviour
{
    public Animator animator; //
    public BoxCollider2D[] platforms; //
    public Rigidbody2D rb; //
    public RaycastHit2D fixRay; //
    public LayerMask layersToHit;
    public Transform rayOrigin;
    public float bouncePadValue;
    static private bool touchingBouncePad, speeding;
    private bool specialWhack;

    public GameObject floorRay;
    public Transform SwingDustTransform;
    ParticleSystem dustParticle;

    public float controllerDeadZone = 0.1f;
    public float horizontalMovementVal;
    public float verticalMovementVal;
    public bool joystickInUse;
    public bool allowTapJump;
    public int jumpForce;
    public int fixMod = 1;
    public static float speedMod = 1f;
    public float horizontalSpeed = 2f;
    public float stunClock, distance, platformClock;
    public static float stunTime = 2.5f;
    public static bool canJump = false;
    public static bool canMove = true;
    public bool platformRotated; //
    public static float buildTimer;
    public float buildClock = 0;
    public bool building = false, stunned = false;
    public string[] materialArray;
    public Sprite[] iconArray;
    public int[] specialBrickAmounts;

    BaseBuilding BaseBuilding;

    // private float oneSecond = 1f;
    public int score;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI materialText;
    public RawImage materialIcon;

    public float yOffset;
    public GameObject floatingText;
    public BoxCollider2D headBox;

    public BoxCollider2D checkBox;

    public AudioSource audioSource;
    public AudioClip walk, jump, brickFix, brickBreak; // haven't found a good walk sound yet

    private GameObject pickaxe, hammer;

    public GameObject SpeedParticles;

    public GameObject JumpParticles;
    public bool SuperJumping;

    public Vector3 targetedBrickTransform;
    private bool teleporterPlaced = false;
    public int brickType = 0;
    // Start is called before the first frame update
    private void Awake()
    {
        dustParticle = GetComponentInChildren<ParticleSystem>();
        dustParticle.Stop();
        buildTimer = 1.3f;
    }
    void Start()
    {
        Debug.Log(this.gameObject.name);
        BaseBuilding = GameObject.FindAnyObjectByType<BaseBuilding>();
        platforms = GameObject.Find("Platforms").GetComponentsInChildren<BoxCollider2D>();
        SwingDustTransform = GameObject.Find("SwingDust").GetComponent<Transform>();
        materialIcon = GameObject.Find("RawImage").GetComponent<RawImage>();
        Physics2D.queriesHitTriggers = true; //making it so that ray can detect triggers
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        jumpForce = 235 * (int)rb.mass;
        horizontalSpeed = 3;
        speedMod = 1;
        speeding = false;
        stunTime = 1.7f;
        yOffset = .5f;
        highScoreText.text = PlayerPrefs.GetInt("HighScore").ToString();
        canJump = true;
        touchingBouncePad = false;
        floatingText = (GameObject)Resources.Load("FloatingTextParent");
        headBox = GameObject.Find("HeadBox").GetComponentInChildren<BoxCollider2D>();
        hammer = GameObject.Find("Hammer");
        hammer.SetActive(false);
        pickaxe = GameObject.Find("Pickaxe");
        pickaxe.SetActive(false);
        joystickInUse = false;
        allowTapJump = false;
        bouncePadValue = 1.7f;

        SpeedParticles = GameObject.Find("Zoom Trails");
        SpeedParticles.SetActive(false);
        JumpParticles = GameObject.Find("Jump Trails");
        JumpParticles.SetActive(false);
        SuperJumping = false;
        canMove = true;
        building = false;
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!animator.GetBool("Swinging") || animator.GetBool("Stun"))
        {
            hammer.SetActive(false);
            pickaxe.SetActive(false);
        }

        if (SuperJumping && canJump)
        {
            SuperJumping = false;
            JumpParticles.SetActive(false);
        }
        if(brickType == 0)
        {
            materialText.text = materialArray[brickType];
        } else
        {
            materialText.text = materialArray[brickType] + ":" + specialBrickAmounts[brickType - 1];
        }

        materialIcon.texture = iconArray[brickType].texture;

        if(teleporterPlaced)
        {
            brickType = 4;
        }

        horizontalMovementVal = Input.GetAxis("Horizontal");
        verticalMovementVal = Input.GetAxis("Vertical");
        if(horizontalMovementVal!=0)
        {
            joystickInUse = true;
        }
        else
        {
            joystickInUse = false;
        }

        if(Input.GetJoystickNames().Length != 0)
        {
            if (Input.GetJoystickNames()[0].Equals("Controller (MAYFLASH Arcade Fightstick F300)"))
            {
                allowTapJump = true;
            }
        }
        
        else
        {
            allowTapJump = false;
        }

        fixRay = Physics2D.Raycast(rayOrigin.position, new Vector2(fixMod , -1), distance, layersToHit);
        Debug.DrawLine(rayOrigin.position, rayOrigin.position + new Vector3(fixMod * distance, -1 * distance)); //visualising ray in editor

        float adjustedSpeed = horizontalSpeed * Time.deltaTime * speedMod;

        //Debug.Log(rb.velocity.y);
        if (canMove && Time.timeScale != 0)
        {
            if (Input.GetKey(KeyCode.A)||horizontalMovementVal < -.1f)
            {
                transform.Translate(-adjustedSpeed, 0, 0, Space.World);

                if (!Input.GetKey(KeyCode.D))
                {
                    fixMod = -1;
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                    SwingDustTransform.rotation = Quaternion.Euler(0, 180, 0);
                }
                animator.SetBool("Walking", true);
                audioSource.clip = walk;
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
            if (Input.GetKey(KeyCode.D) || horizontalMovementVal > .1f)
            {
                transform.Translate(adjustedSpeed, 0, 0, Space.World);
                if (!Input.GetKey(KeyCode.A))
                {
                    fixMod = 1;
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    SwingDustTransform.rotation = Quaternion.Euler(0, 180, 0);
                }
                animator.SetBool("Walking", true);
                audioSource.clip = walk;
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
            if ((!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) || (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)))
            {
                if(!joystickInUse)
                {
                    animator.SetBool("Walking", false);
                }
            }
            if ((Input.GetKeyDown(KeyCode.W)||Input.GetKeyDown("joystick button 0")|| (allowTapJump&&verticalMovementVal > .1f)) && canJump && rb.velocity.y < .25 && rb.velocity.y > -.25 && !(Input.GetKey(KeyCode.S)|| verticalMovementVal < -.1f))
            {
                float bounceMod = 1;
                if (touchingBouncePad)
                    bounceMod = 1.7f;
                //Debug.Log("jump");
                rb.AddForce(new Vector2(0, jumpForce * bounceMod));
                canJump = false;
                animator.SetTrigger("Jump");
                audioSource.PlayOneShot(jump);
                floorRay.SendMessage("JumpAnimCrt");
                foreach (BoxCollider2D platform in platforms)
                {
                    platform.enabled = true;
                }
                if (touchingBouncePad)
                {
                    SuperJumping = true;
                    JumpParticles.SetActive(true);
                }
            }

            if ((Input.GetKeyDown(KeyCode.Space)|| Input.GetKeyDown("joystick button 1")) && canJump && rb.velocity.y < .25 && rb.velocity.y >= 0)
            {
                if (fixRay.collider != null)
                {
                    checkCollider();
                }
            }

            if(BaseBuilding.GameMode == BaseBuilding.Mode.build)
            {
                if (Input.GetKeyDown(KeyCode.E) ||  Input.GetKeyDown("joystick button 2"))
                {
                    do
                    {
                        brickType++;
                        if (brickType > 6)
                        {
                            brickType = 0;
                        }
                    } while (brickType != 0 && specialBrickAmounts[brickType - 1] == 0);
                    /*
                    if(brickType < 4)
                    {
                        brickType++;
                    } else
                    {
                        brickType = 0;
                    }*/
                } else if (Input.GetKeyDown(KeyCode.Q))
                {
                    do
                    {
                        brickType--;
                        if (brickType < 0)
                        {
                            brickType = 6;
                        }
                    } while (brickType != 0 && specialBrickAmounts[brickType-1] == 0);
                    /*
                    if (brickType > 0)
                    {
                        brickType--;
                    }
                    else
                    {
                        brickType = 4;
                    }*/
                }
                if(brickType != 0 && specialBrickAmounts[brickType - 1] == 0)
                    brickType = 0;
            } else
            {
                brickType = 0;
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
            if (BaseBuilding.GameMode != BaseBuilding.Mode.build && BaseBuilding.lastBrickBuilt &&!specialWhack)
            {
                score += 10;
                // Trigger floating text here
                if (floatingText != null)
                {
                    ShowFloatingText("+10");
                }
            }
            else if(specialWhack)
            {
                specialWhack = false;
            }
        }
        scoreText.text = ((int)score).ToString();
        if (score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", (int)score);
            highScoreText.text = PlayerPrefs.GetInt("HighScore").ToString();
        }

        if (fixRay.collider != null && canJump && rb.velocity.y < .25 && rb.velocity.y > -.25)
        {
            if (fixRay.collider.isTrigger)
            {
                if (BaseBuilding.GameMode == BaseBuilding.Mode.defend || (BaseBuilding.GameMode == BaseBuilding.Mode.build && (BaseBuilding.resources - BrickValue() >= 0)) || brickType != 0)
                {
                    fixRay.collider.SendMessage("ShowFixIndicator");
                }
            }
            if (fixRay.collider.gameObject.name.Contains("ShieldGenerator"))
            {
                if (BaseBuilding.GameMode == BaseBuilding.Mode.defend)
                {
                    fixRay.collider.SendMessage("ShowShieldIndicator");
                }
            }
            else if (fixRay.collider.gameObject.name.Contains("Mine"))
            {
                if (BaseBuilding.GameMode == BaseBuilding.Mode.defend)
                {
                    fixRay.collider.SendMessage("ShowMine");
                }
            }
            if (!fixRay.collider.isTrigger && BaseBuilding.GameMode == BaseBuilding.Mode.build )//&& fixRay.collider.gameObject.name.Contains("Brick"))
            {
                fixRay.collider.SendMessage("ShowBreakIndicator");
            }
            targetedBrickTransform = fixRay.collider.gameObject.transform.position;
        }

        //managing green guy particles for special bricks
        if (speeding)
        {
            SpeedParticles.SetActive(true);
        }
        if (!speeding)
        {
            SpeedParticles.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        //score += oneSecond * Time.fixedDeltaTime;
        //scoreText.text = ((int)score).ToString();
    }

    private int BrickValue()
    {
        string parentBrick = fixRay.collider.transform.parent.gameObject.name;
        int brickValue = 0;
        switch (parentBrick) //getting the material value of the targeted brick
        {
            case "Layer 1":
                brickValue = 5;
                break;
            case "Layer 2":
                brickValue = 3;
                break;
            case "Layer 3":
                brickValue = 1;
                break;
            case "Layer 4":
                brickValue = 3;
                break;
            case "Layer 5":
                brickValue = 5;
                break;
        }
        return brickValue;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*if(collision.gameObject.layer == 7) //platforms
        {
            canJump = true;
            animator.SetTrigger("Grounded");
        }*/

        if (collision.gameObject.layer == 6) //ball
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
            string parentBrick = fixRay.collider.transform.parent.gameObject.name;

            if (BaseBuilding.GameMode == BaseBuilding.Mode.build)
            {
                if(brickType == 0)
                {
                    if (BaseBuilding.resources - BrickValue() >= 0)
                    {
                        BaseBuilding.resources -= BrickValue();
                        fixRay.collider.gameObject.SendMessage("fixBrick");
                    }
                    else
                    {
                        return;
                    }
                } else
                {
                    if (specialBrickAmounts[brickType-1] > 0)
                    {
                        fixRay.collider.gameObject.SendMessage("PlaceSpecialBrick", brickType);
                        specialBrickAmounts[brickType-1]--;
                    } else
                    {
                        return;
                    }
                    
                }
            }

            if(BaseBuilding.GameMode == BaseBuilding.Mode.defend)
            {
                fixRay.collider.gameObject.SendMessage("fixBrick");
            }

            animator.SetTrigger("Fix");
            animator.SetBool("Swinging", true);
            hammer.SetActive(true);
            building = true;
            buildClock = 0;
            canMove = false;
            audioSource.PlayOneShot(brickFix);
        }
        else
        {
            if (BaseBuilding.GameMode == BaseBuilding.Mode.build) //destroying bricks in build mode
            {
                fixRay.collider.gameObject.SendMessage("cancelBrick");
                animator.SetTrigger("Fix");
                animator.SetBool("Swinging", true);
                pickaxe.SetActive(true);
                building = true;
                buildClock = 0;
                canMove = false;
                audioSource.PlayOneShot(brickBreak);
                BaseBuilding.resources += BrickValue();
            }
            else if(BaseBuilding.GameMode == BaseBuilding.Mode.defend && fixRay.collider.name.StartsWith("Shield"))
            {
                Shield shield = fixRay.collider.GetComponent<Shield>();
                if (shield.CanStart())
                {
                    specialWhack = true;
                    Debug.Log("Shield Start");
                    animator.SetTrigger("Fix");
                    animator.SetBool("Swinging", true);
                    hammer.SetActive(true);
                    building = true;
                    buildClock = 0;
                    canMove = false;
                    audioSource.PlayOneShot(brickFix);
                    shield.StartShield();
                }
            }
            else if (BaseBuilding.GameMode == BaseBuilding.Mode.defend && fixRay.collider.name.StartsWith("Mine") && !building)
            {
                Mine mine = fixRay.collider.GetComponent<Mine>();
                if (mine.getMineable())
                {
                    specialWhack = true;
                    mine.doAction();
                    ShowFloatingText("+Scrap");
                    animator.SetTrigger("Fix");
                    animator.SetBool("Swinging", true);
                    pickaxe.SetActive(true);
                    building = true;
                    buildClock = 0;
                    canMove = false;
                    audioSource.PlayOneShot(brickBreak);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6 && !building)
        {
            score += 5;
            ShowFloatingText("+5");
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
        GameObject flText = Instantiate(floatingText, transform.position + new Vector3(0, yOffset, 10), Quaternion.identity);
        flText.GetComponentInChildren<TextMesh>().text = points;
    }

    public static void SetSpeeding(bool value)
    {
        speeding = value;
    }

    public static bool GetSpeeding()
    {
        return speeding;
    }

    public static void SetBounce(bool value)
    {
        touchingBouncePad = value;
    }


    public void OnToggleChange(bool tickOn)
    {
        //method to toggle tap jump on and off
        if (tickOn)
        {
            allowTapJump = true;
        }
        else
        {
            allowTapJump = false;
        }
    }

    public void addSpecialResources(int brickInd)
    {
        if (brickInd != 3)
        {
            specialBrickAmounts[brickInd] += 1;
        }
        else
            specialBrickAmounts[brickInd] += 2;
    }

    public void zeroSpecialResources()
    {
        for (int i = 0; i < specialBrickAmounts.Length; i++)
            specialBrickAmounts[i] = 0;
    }
}
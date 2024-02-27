using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class BaseBuilding : MonoBehaviour
{

    public static int resources;
    public TextMeshProUGUI resourcesText;
    public enum Mode { build, defend };
    public static Mode GameMode;
    public static bool lastBrickBuilt;

    private Animator brickAnimator;
    public GameObject ball;
    public GameObject paddle;
    private GameObject[] Bricks;

    private GameObject BuildUI;
    private GameObject DefendUI;
    private GameObject StartButton;

    // Start is called before the first frame update
    void Start()
    {
        Bricks = GameObject.FindGameObjectsWithTag("Brick");
        resources = 75;
        GameMode = Mode.build;
        ball = GameObject.Find("Ball");
        ball.SetActive(false);
        paddle = GameObject.Find("Paddle");
        paddle.SetActive(false);
        BuildUI = GameObject.Find("BuildUI");
        DefendUI = GameObject.Find("DefendUI");
        DefendUI.SetActive(false);
        lastBrickBuilt = false;
        GreenGuy.buildTimer = 0.65f;
    }

    // Update is called once per frame
    void Update()
    {
        resourcesText.text = Convert.ToString(resources);

        if (resources == 0 && GameMode == Mode.build)
        {
            BeginRound();
        }
    }

    /*
    void CountdownToStart()
    {
        if (ctdTimer > 2)
        {
            countdown.text = "3";
        }
        else if (ctdTimer > 1)
        {
            countdown.text = "2";
        }
        else if (ctdTimer > 0)
        {
            countdown.text = "1";
        }
        else
        {
            countdown.text = "";
            ctd =false;
            StartDefend();
        }
    }*/

    public void Spend(int value)
    {
        resources -= value;
    }

    public void BeginRound()
    {
        foreach (GameObject brick in Bricks)
        {
            brick.GetComponent<Animator>().SetFloat("FixMultiplier",.65f);
            if (brick.GetComponent<Animator>().GetBool("IsBroken"))
            {
                brick.SetActive(false);
            }
        }
        GameMode = Mode.defend;
        DefendUI.SetActive(true);
        BuildUI.SetActive(false);
        paddle.SetActive(true);
        ball.SetActive(true);
        ball.GetComponent<Ball>().LaunchSequence();
    }
}

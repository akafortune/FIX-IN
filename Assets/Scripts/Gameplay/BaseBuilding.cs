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

    public TextMeshProUGUI countdown;
    bool ctd;

    float ctdTimer;

    public GameObject ball;
    public GameObject paddle;
    private GameObject[] Bricks;

    private GameObject BuildUI;
    private GameObject DefendUI;
    private GameObject StartButton;

    // Start is called before the first frame update
    void Start()
    {
        resources = 75;
        GameMode = Mode.build;
        countdown.text = "";
        ctd = false;
        ctdTimer = 3.0f;
        ball = GameObject.Find("Ball");
        ball.SetActive(false);
        paddle = GameObject.Find("Paddle");
        paddle.SetActive(false);
        BuildUI = GameObject.Find("BuildUI");
        DefendUI = GameObject.Find("DefendUI");
        DefendUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        resourcesText.text = Convert.ToString(resources);

        if (resources == 0 && GameMode == Mode.build && !ctd)
        {
            BeginRound();
        }
        else if (ctd)
        {
            ctdTimer -= Time.deltaTime;
            CountdownToStart();
        }

    }

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
    }

    void StartDefend()
    {
        ball.GetComponent<Ball>().Launch();
    }


    public void Spend(int value)
    {
        resources -= value;
    }

    public void BeginRound()
    {
        Bricks = GameObject.FindGameObjectsWithTag("Brick");
        foreach (GameObject brick in Bricks)
        {
            if (brick.GetComponent<Animator>().GetBool("IsBroken"))
            {
                brick.SetActive(false);
            }
        }

        ctdTimer = 3.0f;
        GameMode = Mode.defend;
        ctd = true;
        CountdownToStart();
        DefendUI.SetActive(true);
        BuildUI.SetActive(false);
        paddle.SetActive(true);
        ball.SetActive(true);
        ball.GetComponent<Ball>().Rotate();
    }
}

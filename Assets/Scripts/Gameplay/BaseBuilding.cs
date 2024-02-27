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

    // Start is called before the first frame update
    void Start()
    {
        resources = 75;
        GameMode = Mode.build;
        countdown.text = "";
        ctd = false;
        ctdTimer = 3.0f;
    }

    // Update is called once per frame
    void Update()
    {
        resourcesText.text = Convert.ToString(resources);

        if (resources == 0 && GameMode == Mode.build && !ctd)
        {
            Bricks = GameObject.FindGameObjectsWithTag("Brick");
            foreach(GameObject brick in Bricks)
            {
                if (brick.GetComponent<Animator>().GetBool("IsBroken"))
                {
                    brick.SetActive(false);
                }
            }
            ctdTimer = 3.0f;
            ctd = true;
            CountdownToStart();
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
            ball.SetActive(true);
            paddle.SetActive(true);
            GameMode = Mode.defend;
    }


    public void Spend(int value)
    {
        resources -= value;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class RoundTimeManager : MonoBehaviour
{
    public Ball ball;

    // countdown variables for the ball
    public float ctdTimer;
    public bool countingDown;
    public TextMeshProUGUI countdownText;

    // variables for the on screen timer
    public float currentTime; // the current time of the countdown in a round
    public float roundTime; // the amount of time that a round lasts for
    //public float roundClock;
    public TextMeshProUGUI timerText; // the on screen timer itself

    public float IronRndTime, HeadRndTime; //times for the freaky rounds

    public Color regularColor = Color.white;
    public Color thirtySeconds;
    public Color tenSeconds;

    // round count stuff
    public TextMeshProUGUI roundCountText;
    public static float roundCount = 0;

    public RoundManager rndmng;

    // Start is called before the first frame update
    void Awake()
    {
        ball = GameObject.Find("Ball").GetComponent<Ball>();
        timerText = GameObject.Find("TimerText").GetComponent<TextMeshProUGUI>();
        //roundTime = 60f;
        currentTime = roundTime;
        ctdTimer = 3.5f;
        countdownText = GameObject.Find("Countdown").GetComponent<TextMeshProUGUI>();
        roundCountText = GameObject.Find("RoundNumberText").GetComponent<TextMeshProUGUI>();
        rndmng = GameObject.Find("Bricks").GetComponent<RoundManager>();
        roundCount = 5;

        IronRndTime = 30f;
        HeadRndTime = 30f;
    }

    // Update is called once per frame
    void Update()
    {
        // for the first launch of the round
        //if(RoundManager.GameMode == RoundManager.Mode.build)
        //{
        //    ball.SetFirstLaunch(true);
        //}

        roundCountText.text = roundCount.ToString();

        // for the on screen timer 
        if (timerText.IsActive() && RoundManager.GameMode == RoundManager.Mode.defend)
        {
            TimeSpan time;
            //Debug.Log("Time: " + currentTime);
            currentTime -= Time.deltaTime;
            time = TimeSpan.FromSeconds(currentTime);
            timerText.text = time.ToString(@"mm\:ss");
            if (currentTime <= 0f)
            {
                //currentTime = roundTime;
                timerText.gameObject.SetActive(false); // stops the current time from decreasing before launch
                rndmng.BeginBuild();
            }

            //timeText.alpha = 1f;
            //changes colors depending on the current time
            if (currentTime <= roundTime / 2 && currentTime > roundTime / 6)
            {
                timerText.color = thirtySeconds;
            }
            else if (currentTime <= roundTime/6)
            {
                timerText.color = tenSeconds;
            }
            else
            {
                timerText.color = regularColor;
            }
        }


        //if (countingDown)
        //    roundClock += Time.deltaTime;

        //if(roundClock >= roundTime)
        
    }

    public void ToggleRoundTimer(bool tf)
    {
        timerText.gameObject.SetActive(tf);
    }

    public void ToggleCountdownText(bool tf)
    {
        countdownText.gameObject.SetActive(tf);
    }

    //// gets the current round time
    //public float getCurrentTime()
    //{
    //    return currentTime;
    //}

    //// sets current round time
    //public void setCurrentTIme(float newTime)
    //{
    //    currentTime = newTime;
    //}

    //// gets the countdown time
    //public float getCountDownTime()
    //{
    //    return ctdTimer;
    //}

    //// sets the countdown time
    //public void setCountDownTime(float newTime)
    //{
    //    ctdTimer = newTime;
    //}

    //// gets the bool for counting down
    //public bool isCountingDown()
    //{
    //    return countingDown;
    //}

    //// setting the countdown bool
    //public void setCountingDown(bool tf)
    //{
    //    countingDown = tf;
    //}


    //public void setCountDownText(string text)
    //{
    //    countdownText.text = text;
    //}
}

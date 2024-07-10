using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeInGame : MonoBehaviour
{

    float currentTime;
    public TextMeshProUGUI timeText;
    public Renderer timerRend;
    public Color regularColor = Color.white;
    public Color thirtySeconds;
    public Color tenSeconds;

    //private float oneSecond = 1f;
    //public float score;
    //public TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = 60f;
        //thirtySeconds = (253f, 150f, 0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (timeText.IsActive() && RoundManager.GameMode == RoundManager.Mode.defend)
        {
            TimeSpan time;
            //Debug.Log("Time: " + currentTime);
            currentTime -= Time.deltaTime;
            time = TimeSpan.FromSeconds(currentTime);
            timeText.text = time.ToString(@"mm\:ss");
            if (currentTime <= 0f)
            {
                currentTime = 60f;
            }
            //timeText.alpha = 1f;
            if (currentTime <= 30f && currentTime > 10f)
                timeText.color = thirtySeconds;
            else if (currentTime <= 10f)
                timeText.color = tenSeconds;
            else
            {
                timeText.color = regularColor;
            }
        }
    }
    void FixedUpdate()
    {
        if (RoundManager.GameMode == RoundManager.Mode.defend)
        {
            //score += oneSecond * Time.fixedDeltaTime;
            //scoreText.text = ((int)score).ToString();
        }
    }

    public float getCurrentTime()
    {
        return currentTime;
    }

}

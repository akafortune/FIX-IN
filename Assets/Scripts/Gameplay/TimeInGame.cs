using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeInGame : MonoBehaviour
{

    float currentTime;
    public TextMeshProUGUI timeText;

    private float oneSecond = 1f;
    public float score;
    public TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = 60f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeText.IsActive() && BaseBuilding.GameMode == BaseBuilding.Mode.defend)
        {
            TimeSpan time;
            //Debug.Log("Time: " + currentTime);
            currentTime -= Time.deltaTime;
            time = TimeSpan.FromSeconds(currentTime);
            timeText.text = time.ToString(@"mm\:ss");
            if (currentTime <= 0f)
                currentTime = 60f;
        }
    }
    void FixedUpdate()
    {
        if (BaseBuilding.GameMode == BaseBuilding.Mode.defend)
        {
            score += oneSecond * Time.fixedDeltaTime;
            scoreText.text = ((int)score).ToString();
        }
    }

    public float getCurrentTime()
    {
        return currentTime;
    }

}

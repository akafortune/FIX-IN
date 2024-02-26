using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static BaseBuilding;

public class TimeInGame : MonoBehaviour
{
    public BaseBuilding bb;

    float currentTime;
    public TextMeshProUGUI timeText;

    private float oneSecond = 1f;
    public float score;
    public TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (resources <= 0)
        {
            TimeSpan time;
            currentTime += Time.deltaTime;
            time = TimeSpan.FromSeconds(currentTime);
            timeText.text = time.ToString(@"mm\:ss");
        }
    }
    void FixedUpdate()
    {
        if (resources <= 0)
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

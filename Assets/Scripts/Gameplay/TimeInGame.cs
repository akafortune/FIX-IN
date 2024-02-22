using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeInGame : MonoBehaviour
{
    float currentTime;
    public TextMeshProUGUI timeText;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        TimeSpan time;
        currentTime += Time.deltaTime;
        time = TimeSpan.FromSeconds(currentTime);
        timeText.text = time.ToString(@"mm\:ss");
    }

    public float getCurrentTime()
    {
        return currentTime;
    }

}

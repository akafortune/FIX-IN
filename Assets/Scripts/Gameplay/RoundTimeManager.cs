using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundTimeManager : MonoBehaviour
{
    public Ball ball;

    // Start is called before the first frame update
    void Awake()
    {
        ball = GameObject.Find("Ball").GetComponent<Ball>();
    }

    // Update is called once per frame
    void Update()
    {
        if(RoundManager.GameMode == RoundManager.Mode.build)
        {
            ball.SetFirstLaunch(true);
        }
    }
}

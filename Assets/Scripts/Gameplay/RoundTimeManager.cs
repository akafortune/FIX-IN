using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundTimeManager : MonoBehaviour
{
    public Ball ball;

    // Start is called before the first frame update
    void Start()
    {
        ball = GameObject.Find("Ball").GetComponent<Ball>();
    }

    // Update is called once per frame
    void Update()
    {
        if(BaseBuilding.GameMode == BaseBuilding.Mode.build)
        {
            ball.SetFirstLaunch(true);
        }
    }
}

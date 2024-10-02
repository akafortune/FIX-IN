using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TutorialBallCatcher : MonoBehaviour
{
    [SerializeField]
    private TutorialManager tm;
    [SerializeField]
    private Ball ball;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == ball.gameObject)
        {
            ball.ResetBall();
            ball.gameObject.SetActive(false);
            tm.BallCaught();
        }
    }
}

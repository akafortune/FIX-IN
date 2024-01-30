using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    Transform Ball;
    // Start is called before the first frame update
    void Start()
    {
        Ball = GameObject.Find("Ball").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Ball.position.x;
        x = Mathf.Clamp(x, -2.25f, 2.25f);
        transform.position = new Vector3(x, 4.25f, 0);
    }
}

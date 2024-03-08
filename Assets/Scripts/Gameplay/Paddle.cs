using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Paddle : MonoBehaviour
{
    Transform Ball;
    public Animator Anim;
    float Xclamp;
    // Start is called before the first frame update
    void Start()
    {
        Ball = GameObject.Find("Ball").GetComponent<Transform>();

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(3) || SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(4))
        {
            Xclamp = 4.25f;
        }
        else
        {
            Xclamp = 2.25f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float x = Ball.position.x;
        x = Mathf.Clamp(x, -Xclamp, Xclamp);
        transform.position = new Vector3(x, 4.25f, 0);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            Anim.SetTrigger("Hit");
        }
    }
}

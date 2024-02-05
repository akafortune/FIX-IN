using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    private float oneSecond = 1f;
    public float score;
    public TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            score += 10;
        }
        scoreText.text = ((int)score).ToString();
    }

    private void FixedUpdate()
    {
        //score += oneSecond * Time.fixedDeltaTime;
        scoreText.text = ((int)score).ToString();
    }

}

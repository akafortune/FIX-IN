using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCageLoop : MonoBehaviour
{
    public GameObject ballCage;

    float timer; 
    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0)
        {
            GameObject newCage = Instantiate(ballCage, gameObject.transform);
            newCage.GetComponent<RectTransform>().anchoredPosition = new Vector2(91f, -22f);
            timer = 30f;
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }
}

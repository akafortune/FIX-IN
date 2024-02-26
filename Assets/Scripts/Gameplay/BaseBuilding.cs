using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class BaseBuilding : MonoBehaviour
{

    public static int resources;
    public TextMeshProUGUI resourcesText;


    public GameObject ball;
    public GameObject paddle;

    // Start is called before the first frame update
    void Start()
    {
        resources = 50;
    }

    // Update is called once per frame
    void Update()
    {
        resourcesText.text = Convert.ToString(resources);

        if (resources <= 0)
        {
            ball.SetActive(true);
            paddle.SetActive(true);
        }

    }


    public void Spend(int value)
    {
        resources -= value;
    }
}

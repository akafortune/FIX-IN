using System;
using TMPro;
using UnityEngine;

public class BaseBuilding : MonoBehaviour
{

    public static int resources;
    public TextMeshProUGUI resourcesText;
    public enum Mode { build, defend };
    public static Mode GameMode;


    public GameObject ball;
    public GameObject paddle;

    // Start is called before the first frame update
    void Start()
    {
        resources = 50;
        GameMode = Mode.build;
    }

    // Update is called once per frame
    void Update()
    {
        resourcesText.text = Convert.ToString(resources);

        if (resources == 0)
        {
            ball.SetActive(true);
            paddle.SetActive(true);
            GameMode = Mode.defend;
        }

    }


    public void Spend(int value)
    {
        resources -= value;
    }
}

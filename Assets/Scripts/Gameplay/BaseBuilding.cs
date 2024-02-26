using System;
using System.Linq;
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
    private GameObject[] Bricks;

    // Start is called before the first frame update
    void Start()
    {
        resources = 5;
        GameMode = Mode.build;
        Bricks = GameObject.FindGameObjectsWithTag("Brick");
    }

    // Update is called once per frame
    void Update()
    {
        resourcesText.text = Convert.ToString(resources);

        if (resources == 0 && GameMode == Mode.build)
        {
            ball.SetActive(true);
            paddle.SetActive(true);
            GameMode = Mode.defend;
            foreach(GameObject brick in Bricks)
            {
                if (brick.GetComponent<Animator>().GetBool("IsBroken"))
                {
                    brick.SetActive(false);
                }
            }    
        }

    }


    public void Spend(int value)
    {
        resources -= value;
    }
}

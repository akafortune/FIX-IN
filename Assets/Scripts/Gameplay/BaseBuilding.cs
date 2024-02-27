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
        resources = 75;
        GameMode = Mode.build;
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
            Bricks = GameObject.FindGameObjectsWithTag("Brick");
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

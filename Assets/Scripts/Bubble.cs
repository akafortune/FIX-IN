using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Bubble : MonoBehaviour
{
    public GameObject parent;
    public GameObject[] brickTypes;
    private GameObject otherBubble;
    public Transform brickHolder;
    public int brickInd;
    public int cost;
    public TextMeshPro text;
    protected static int previousBrickInd = -1;

    void Start()
    {
        if(parent.transform.parent.name.Equals("Bubble 1"))
        {
            otherBubble = GameObject.Find("Bubble 2").transform.GetChild(0).gameObject;
        }
        if (parent.transform.parent.name.Equals("Bubble 2"))
        {
            otherBubble = GameObject.Find("Bubble 1").transform.GetChild(0).gameObject;
        }
        GameObject brickIcon = Instantiate(brickTypes[brickInd], brickHolder);

        switch (brickInd)
        {
            case 0:
                text.text = "Bounce Pad\n-5 Resources";
                cost = 5;
                break;
            case 1:
                text.text = "Shield Generator\n-20 Resources";
                cost = 20;
                break;
            case 2:
                text.text = "Speed Pad\n-10 Resources";
                cost = 10;
                break;
            case 3:
                text.text = "Teleporter\n-15 Resources";
                cost = 15;
                break;
            case 4:
                text.text = "Mine\n-15 Resources";
                cost = 15;
                break;
            case 5:
                text.text = "Reinforced Brick\n-5 Resources";
                cost = 5;
                break;
            default:
                break;
        }
            
    }

    private void Update()
    {
        if(BaseBuilding.GameMode == BaseBuilding.Mode.defend)
        {
            //StartCoroutine(Pop());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.name.Equals("HeadBox") && BaseBuilding.resources - cost >= 0)
        {
            collision.transform.parent.SendMessage("addSpecialResources", brickInd);
            BaseBuilding.resources -= cost;

            BaseBuilding.justGot = true;

            StartCoroutine(Pop());
        }
    }

    public void StartPop()
    {
        StartCoroutine(Pop());
    }
    IEnumerator Pop()
    {
        gameObject.GetComponent<Collider2D>().enabled = false;
        parent.GetComponent<Animator>().SetTrigger("Pop");
        otherBubble.GetComponent<Animator>().SetTrigger("Pop");
        Destroy(brickHolder.gameObject);
        Destroy(otherBubble.transform.GetChild(5).gameObject);
        yield return new WaitForSeconds(.5f);
        Destroy(parent);
        Destroy(otherBubble);
    }
}

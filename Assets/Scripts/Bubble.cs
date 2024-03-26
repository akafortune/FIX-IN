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
        cost = 15;

        switch (brickInd)
        {
            case 0:
                text.text += "Bounce Pad";
                break;
            case 1:
                text.text += "Shield Generator";
                break;
            case 2:
                text.text += "Speed Pad";
                break;
            case 3:
                text.text += "Teleporter";
                foreach(Transform child in brickIcon.GetComponentInChildren<Transform>())
                    child.gameObject.SetActive(false);
                break;
            default:
                break;
        }
            
    }

    private void Update()
    {
        if(BaseBuilding.GameMode == BaseBuilding.Mode.defend)
        {
            Destroy(otherBubble);
            Destroy(parent);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("HeadBox") && BaseBuilding.resources - cost >= 0)
        {
            collision.transform.parent.SendMessage("addSpecialResources", brickInd);
            BaseBuilding.resources -= cost;
            Destroy(otherBubble);
            Destroy(parent);
        }
    }
}

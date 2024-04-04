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
                text.text = "-5 Resources \n\n\n\n\n\n\nBounce Pad";
                cost = 5;
                break;
            case 1:
                text.text = "-20 Resources \n\n\n\n\n\n\nShield Generator";
                cost = 20;
                break;
            case 2:
                text.text = "-10 Resources \n\n\n\n\n\n\nSpeed Pad";
                cost = 10;
                break;
            case 3:
                text.text = "-15 Resources \n\n\n\n\n\n\nTeleporter";
                foreach(Transform child in brickIcon.GetComponentInChildren<Transform>())
                    child.gameObject.SetActive(false);
                cost = 15;
                break;
            case 4:
                text.text = "-15 Resources \n\n\n\n\n\n\nMine";
                cost = 15;
                break;
            case 5:
                text.text = "-5 Resources \n\n\n\n\n\n\nReinforced Brick";
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

            StartCoroutine(Pop());
            
        }
    }

    IEnumerator Pop()
    {
        gameObject.GetComponent<Collider2D>().enabled = false;
        Destroy(otherBubble);
        parent.GetComponent<Animator>().SetTrigger("Pop");
        Destroy(brickHolder.gameObject);
        yield return new WaitForSeconds(.5f);
        Destroy(parent);
    }
}

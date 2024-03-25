using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Bubble : MonoBehaviour
{
    public GameObject parent;
    public GameObject[] brickTypes;
    public Transform brickHolder;
    public int brickInd;
    public int cost;
    public TextMeshPro text;

    void Start()
    {
        brickInd = Random.Range(0, brickTypes.Length);

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
                break;
            default:
                break;
        }
            
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("GreenGuy") && BaseBuilding.resources - cost >= 0)
        {
            collision.gameObject.SendMessage("addSpecialResources", brickInd);
            BaseBuilding.resources -= cost;
            parent.SetActive(false);
        }
    }
}

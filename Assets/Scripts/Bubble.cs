using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public GameObject parent;
    public GameObject[] brickTypes;
    public Transform brickHolder;
    public int brickInd;

    void Start()
    {
        brickInd = Random.Range(0, brickTypes.Length);

        GameObject brickIcon = Instantiate(brickTypes[brickInd], brickHolder);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("GreenGuy"))
        {
            collision.gameObject.SendMessage("addSpecialResources", brickInd);
            parent.SetActive(false);
        }
    }
}

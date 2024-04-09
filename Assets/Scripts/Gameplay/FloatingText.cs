using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public float destroyTime = 1f;
    public float yOffset;
    private Animator anim;
    public GameObject floatingText;
    public GameObject greenGuy;

    // Start is called before the first frame update
    void Start()
    {
        floatingText = (GameObject)Resources.Load("FloatingTextParent");
        yOffset = 0.5f;
        greenGuy = GameObject.Find("GreenGuy");
        //Destroy(floatingText, destroyTime);
        //transform.localPosition += new Vector3(0, yOffset, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowFloatingText(string points)
    {
        // text pop up should appear in the right direction no matter where the player faces
        GameObject flText = Instantiate(floatingText, greenGuy.transform.position + new Vector3(0, yOffset, 0), Quaternion.identity);
        flText.GetComponentInChildren<TextMesh>().text = points;
        Destroy(flText, destroyTime);
    }
}

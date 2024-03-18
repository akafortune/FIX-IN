using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Parry : MonoBehaviour
{
    public float yOffset;
    public GameObject floatingText;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        floatingText = (GameObject)Resources.Load("FloatingTextParent");
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.layer == 6)
    //    {
    //        ShowFloatingText("EPIC!");
    //    }
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            ShowFloatingText("EPIC!");
            //audioSource.PlayOneShot(audioSource.clip);
        }
    }

    public void ShowFloatingText(string points)
    {
        // text pop up should appear in the right direction no matter where the player faces
        GameObject flText = Instantiate(floatingText, transform.position + new Vector3(0, yOffset, 10), Quaternion.identity);
        flText.GetComponentInChildren<TextMesh>().text = points;
    }
}

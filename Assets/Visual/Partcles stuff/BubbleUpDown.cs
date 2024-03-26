using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleUpDown : MonoBehaviour
{
    public float timer = 5f;
    public bool up = true;
   
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                if (up)
                {
                    gameObject.transform.position += new Vector3(0, 0.0005f, 0);
                }
                if (!up)
                {
                    gameObject.transform.position -= new Vector3(0, 0.0005f, 0);
                }
            }
            else
            {
                timer = 5f;
                up = !up;
            }
        }
    }

    
}

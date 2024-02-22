using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public float destroyTime = 3f;
    public float yOffset;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destroyTime);
        //transform.localPosition += new Vector3(0, yOffset, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

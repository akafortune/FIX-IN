using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelVisuals : MonoBehaviour
{
    public float spinYDelta, spinXDelta, spinZDelta;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(spinXDelta, spinYDelta, spinZDelta);
    }
}

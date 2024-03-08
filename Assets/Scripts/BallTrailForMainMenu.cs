using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTrailForMainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Transform Target = GameObject.Find("TargetGreenGuy").GetComponent<Transform>();
        gameObject.transform.position = new Vector3(Target.position.x, 2.42f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

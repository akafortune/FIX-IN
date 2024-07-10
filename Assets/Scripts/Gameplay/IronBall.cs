using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IronBall : Ball
{
    private new void Start()
    {
        ImportData();
        base.Start();
    }

    private new void Update()
    {
        hits = 0;
        base.Update();
        trail.startColor = Color.gray;
    }
}

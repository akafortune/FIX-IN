using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyBall : Ball
{
    Color trailColor;
        private new void Start()
        {
            ImportData();
            trailColor = trail.startColor;
            base.Start();
        }

        private new void Update()
        {
            hits = 0;
            base.Update();
            trail.startColor = trailColor;
        }
}

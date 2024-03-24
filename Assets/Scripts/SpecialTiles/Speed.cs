using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speed : SpecialTile
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        effectLength = 5;
    }

    protected override void OnTriggerStay2D(Collider2D collision)
    {
        if(!(Input.GetAxis("Vertical") < -.1f))
            base.OnTriggerStay2D(collision);
    }
    protected override void doAction()
    {
        Debug.Log("trigger");
        GreenGuy.speedMod = 1.5f;
        GreenGuy.SetSpeeding(true);
    }

    protected override void stopAction()
    {
        GreenGuy.speedMod = 1;
        GreenGuy.SetSpeeding(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speed : SpecialTile
{
    // Start is called before the first frame update
    void Start()
    {
        effectLength = 2;
    }

    protected override void doAction()
    {
        Debug.Log("trigger");
        GreenGuy.speedMod = 1.5f;
    }

    protected override void stopAction()
    {
        GreenGuy.speedMod = 1;
    }
}

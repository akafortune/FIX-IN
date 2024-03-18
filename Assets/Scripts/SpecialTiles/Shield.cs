using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : SpecialTile
{
    GameObject shield;
    // Start is called before the first frame update
    void Start()
    {
        shield = transform.GetChild(0).gameObject;
        effectLength = 3;
    }

    protected override void doAction()
    {
        shield.SetActive(true);
    }

    protected override void stopAction()
    {
        shield.SetActive(false);
    }
}

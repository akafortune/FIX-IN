using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static Brick;

public class BrickLayers : MonoBehaviour
{
    public Material colorMatA;
    public Material colorMatB;
    public GameObject bricks;
    // Start is called before the first frame update
    void Awake()
    {
        int brickNumber = 1;
        int rowNumber = name.ToCharArray()[name.Length-1] -48; //correcting for weird char shenanigans

        Material[] mats = Resources.LoadAll<Material>("BrickMats/");
        bricks = (GameObject)Resources.Load("Brick");
        if (rowNumber %2 == 1)
        {
            colorMatA = mats[0];
            colorMatB = mats[1];
        }
        else
        {
            colorMatA = mats[1];
            colorMatB = mats[0];
        }

        for (float f = -4.5f; f < 5f; f+=1f)
        {
            GameObject i = Instantiate(bricks, gameObject.transform);
            i.transform.localPosition = new Vector3(f, 0, 0);
            SpriteRenderer s =  i.GetComponent<SpriteRenderer>();
            if(brickNumber % 2 == 1)
            {
                s.material = colorMatA;
            }
            else
            {
                s.material = colorMatB;
            }
            //s.material = colorMatA;
            Brick b = i.GetComponent<Brick>();
            b.StartForBuild();
            brickNumber++;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Brick;

public class BrickLayers : MonoBehaviour
{
    public Material colorMat;
    public GameObject bricks;
    // Start is called before the first frame update
    void Start()
    {
        for (float f = -4.5f; f < 5f; f+=1f)
        {
            GameObject i = Instantiate(bricks, gameObject.transform);
            i.transform.localPosition = new Vector3(f, 0, 0);
            SpriteRenderer s =  i.GetComponent<SpriteRenderer>();
            s.material = colorMat;
            Brick b = i.GetComponent<Brick>();
            b.StartForBuild();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

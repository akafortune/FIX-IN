using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tumble : MonoBehaviour
{
    RectTransform tf;
    // Start is called before the first frame update
    void Start()
    {
        tf = gameObject.GetComponent<RectTransform>();
        Destroy(gameObject, 5);
    }

    // Update is called once per frame
    void Update()
    {
        tf.anchoredPosition -= new Vector2(Time.deltaTime*50, 0);
    }
}

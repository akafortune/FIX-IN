using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreStatic : MonoBehaviour
{
    public static int FinalScore;
    public static string FinalName;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}

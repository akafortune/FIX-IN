using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreName : MonoBehaviour
{
    char[] letters;
    TextMeshProUGUI Letter1, Letter2, Letter3;

    // Start is called before the first frame update
    void Start()
    {
        char[] StartingLetters = {'A', 'A', 'A'};
        letters = StartingLetters;
        Letter1 = GameObject.Find("Letter 1").GetComponent<TextMeshProUGUI>();
        Letter2 = GameObject.Find("Letter 2").GetComponent<TextMeshProUGUI>();
        Letter3 = GameObject.Find("Letter 3").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        Letter1.text = letters[0].ToString();
        Letter2.text = letters[1].ToString();
        Letter3.text = letters[2].ToString();
    }

    public void IncrementLetter(int letterindex)
    {
        if (letters[letterindex] == 'Z')
            letters[letterindex] = 'A';
        else
            letters[letterindex]++;
    }

    public void DecrementLetter(int letterindex)
    {
        if (letters[letterindex] == 'A')
            letters[letterindex] = 'Z';
        else
            letters[letterindex]--;
    }
}

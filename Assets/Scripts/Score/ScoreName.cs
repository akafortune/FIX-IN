using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreName : MonoBehaviour
{
    public char[] letters;
    string name;
    public static int scoreIndex = 4;
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
        name = letters[0].ToString() + letters[1].ToString() + letters[2].ToString();
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

    public void Restart()
    {
        SetScores();
        MainMenu menu = GameObject.FindAnyObjectByType<MainMenu>();
        menu.LoadScene(3);
    }

    public void Menu()
    {
        SetScores();
        MainMenu menu = GameObject.FindAnyObjectByType<MainMenu>();
        menu.LoadScene(0);
    }

    public void SetScores()
    {
        HighScoresSet scoresSet = GameObject.FindAnyObjectByType<HighScoresSet>();
        if (scoreIndex == 0)
        {
            StreamWriter scoreWriter = new StreamWriter(Application.persistentDataPath + "/SaveData/Scores.txt");
            scoreWriter.WriteLine(name + " GUY" + "," + scoresSet.scores[0].points);
            scoreWriter.WriteLine(scoresSet.scores[1].name + "," + scoresSet.scores[1].points);
            scoreWriter.WriteLine(scoresSet.scores[2].name + "," + scoresSet.scores[2].points);
            scoreWriter.Close();
        }
        else if (scoreIndex == 1)
        {
            StreamWriter scoreWriter = new StreamWriter(Application.persistentDataPath + "/SaveData/Scores.txt");
            scoreWriter.WriteLine(scoresSet.scores[0].name + "," + scoresSet.scores[0].points);
            scoreWriter.WriteLine(name + " GUY" + "," + scoresSet.scores[1].points);
            scoreWriter.WriteLine(scoresSet.scores[2].name + "," + scoresSet.scores[2].points);
            scoreWriter.Close();
        }
        else if (scoreIndex == 2)
        {
            StreamWriter scoreWriter = new StreamWriter(Application.persistentDataPath + "/SaveData/Scores.txt");
            scoreWriter.WriteLine(scoresSet.scores[0].name + "," + scoresSet.scores[0].points);
            scoreWriter.WriteLine(scoresSet.scores[1].name + "," + scoresSet.scores[1].points);
            scoreWriter.WriteLine(name + " GUY" + "," + scoresSet.scores[2].points);
            scoreWriter.Close();;
        }
    }
}

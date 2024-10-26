using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;
using File = System.IO.File;

public class HighScoresSet : MonoBehaviour
{
    public Score[] scores = new Score[3];
    private TextMeshProUGUI highScoreText;
    bool firstFrame;
    // Start is called before the first frame update
    void Awake()
    {
        highScoreText = GameObject.Find("HighScores").GetComponent<TextMeshProUGUI>();
        string scoreDataPath = Application.persistentDataPath + "/SaveData/Scores.txt";
        if (!File.Exists(scoreDataPath))
        {
            StreamWriter scoreWriter = new StreamWriter(scoreDataPath);
            scoreWriter.WriteLine("AAA GUY,0");
            scoreWriter.WriteLine("AAA GUY,0");
            scoreWriter.WriteLine("AAA GUY,0");
            scoreWriter.Close();
        }

        StreamReader scoreReader = new StreamReader(scoreDataPath);
        string[] inputs = new string[3];
        inputs[0] = scoreReader.ReadLine();
        inputs[1] = scoreReader.ReadLine();
        inputs[2] = scoreReader.ReadLine();
        scoreReader.Close();

        int index = 0;
        scores = new Score[3];
        firstFrame = true;

        foreach (string input in inputs)
        {
            string[] splitInput = input.Split(',');
            print(splitInput[0]);
            scores[index] = new Score(Convert.ToInt32(splitInput[1]), splitInput[0]);
            index++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        highScoreText.text = "High Scores\n\n" + scores[0].toString() + "\n" + scores[1].toString() + "\n" + scores[2].toString();    
    }

    
}

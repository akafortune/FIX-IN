using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public int scoreToGrow;
    public int currentScore;
    public int highScoreValue;
    public int scoreGrowthRate = 1;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = ((int)scoreToGrow).ToString();
        if (currentScore > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", (int)scoreToGrow);
        }

        highScoreText.text = PlayerPrefs.GetInt("HighScore").ToString();
        highScoreValue = PlayerPrefs.GetInt("HighScore");

        if (scoreToGrow != currentScore && scoreToGrow < currentScore)
        {
            scoreToGrow += scoreGrowthRate;
        }
    }

    public void IncreaseScore(int points)
    {
        currentScore += points;
    }

    public void ResetHighScore()
    {
        PlayerPrefs.SetInt("HighScore", 0);
    }

    public void GameOver()
    {
        GameStats.playerScore = currentScore;
        GameStats.roundsLasted = BaseBuilding.getRoundCount();
        if (currentScore <= highScoreValue)
        {
            print("High Score is higher than current score");
            SceneManager.LoadScene("GameOver");
        }
        // when the player loses and does get a new high score
        //else { }
    }
}

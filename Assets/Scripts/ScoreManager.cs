using System;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager: MonoBehaviour
{
    [SerializeField] public Text scoreText;
    
    public static int score;
    private static int _profileHighScore;
    private int _profileSelected;

    private void Start()
    {
        _profileSelected = ProfileSelectMenu.profileChosen;
        _profileHighScore = PlayerPrefs.GetInt("profile" + _profileSelected + "Highscore");

        if (GameBoard.watchReplaySelected)
        {
            score = 0;
        }
    }

    private void Update()
    {
        scoreText.text = score.ToString();
        
        if (!GameBoard.watchReplaySelected)
        {
            if (score > _profileHighScore)
            {
                _profileHighScore = score;
            
                PlayerPrefs.SetInt("profile" + _profileSelected + "Highscore", _profileHighScore);
                PlayerPrefs.Save();
            }
        }
    }
}
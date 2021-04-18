using System;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager: MonoBehaviour
{
    [SerializeField] public Text scoreText;
    public static int profile1Score;
    private static int _profile1HighScore;

    private void Start()
    {
        _profile1HighScore = PlayerPrefs.GetInt("profile1highscore", _profile1HighScore);
    }

    private void Update()
    {
        scoreText.text = profile1Score.ToString();
        
        if (profile1Score > _profile1HighScore)
        {
            _profile1HighScore = profile1Score;
            
            PlayerPrefs.SetInt("profile1highscore", _profile1HighScore);
            PlayerPrefs.Save();
        }
    }
}
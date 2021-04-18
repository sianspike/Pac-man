using System;
using UnityEngine;
using UnityEngine.UI;

public class HighScores: MonoBehaviour
{
        private int _player1HighScore;
        [SerializeField] public Text profile1ScoreText;

        private void Start()
        {
                _player1HighScore = PlayerPrefs.GetInt("profile1highscore", _player1HighScore);
                profile1ScoreText.text = _player1HighScore.ToString();
        }
}
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HighScores: MonoBehaviour
{
        [SerializeField] public Text profile1ScoreText;
        [SerializeField] public Text profile2ScoreText;
        [SerializeField] public Text profile3ScoreText;
        [SerializeField] public Text profile1NameText;
        [SerializeField] public Text profile2NameText;
        [SerializeField] public Text profile3NameText;

        private void Start()
        {
                if (PlayerPrefs.HasKey("profile1Highscore") && PlayerPrefs.HasKey("profile1Name"))
                {
                        profile1NameText.text = PlayerPrefs.GetString("profile1Name");
                        profile1ScoreText.text = PlayerPrefs.GetInt("profile1Highscore").ToString();
                }

                if (PlayerPrefs.HasKey("profile2Highscore") && PlayerPrefs.HasKey("profile2Name"))
                {
                        profile2NameText.text = PlayerPrefs.GetString("profile2Name");
                        profile2ScoreText.text = PlayerPrefs.GetInt("profile2Highscore").ToString();
                }

                if (PlayerPrefs.HasKey("profile3Highscore") && PlayerPrefs.HasKey("profile3Name"))
                {
                        profile3NameText.text = PlayerPrefs.GetString("profile3Name");
                        profile3ScoreText.text = PlayerPrefs.GetInt("profile3Highscore").ToString();
                }
        }

        private void Update()
        {
                if (Input.GetKeyUp(KeyCode.Return))
                {
                        SceneManager.LoadScene("GameMenu");
                }
        }
}
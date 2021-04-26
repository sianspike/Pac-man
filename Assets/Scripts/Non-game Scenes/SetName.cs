using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SetName: MonoBehaviour
{
        [SerializeField] public InputField nameInputField;
        [SerializeField] public Text levelText;

        private int _profileSlot;
        private TransitionManager _transition;
        private int _chosenLevel = 1;

        private void Start()
        {
                _profileSlot = ProfileSelectMenu.profileChosen;
                _transition = FindObjectOfType<TransitionManager>();
                nameInputField.Select();
        }

        private void Update()
        {
                UserInput();
        }

        private void UserInput()
        {
                if (Input.GetKeyUp(KeyCode.Return))
                {
                        PlayerPrefs.SetString(("profile" + _profileSlot + "Name"), nameInputField.text);
                        PlayerPrefs.SetInt("profile" + _profileSlot + "Level", _chosenLevel);
                        PlayerPrefs.Save();

                        StartCoroutine(_transition.PlayTransition());
                        SceneManager.LoadScene("Level1");
                        
                } else if (Input.GetKeyUp(KeyCode.RightArrow))
                {
                        _chosenLevel++;
                        levelText.text = _chosenLevel.ToString();
                        
                } else if (Input.GetKeyUp(KeyCode.LeftArrow))
                {
                        if (_chosenLevel > 1)
                        {
                                _chosenLevel--;
                        }

                        levelText.text = _chosenLevel.ToString();
                }
        }
}
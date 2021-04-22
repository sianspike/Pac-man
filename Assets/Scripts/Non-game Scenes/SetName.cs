using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SetName: MonoBehaviour
{
        [SerializeField] public InputField nameInputField;

        private int _profileSlot;
        private TransitionManager _transition;

        private void Start()
        {
                _profileSlot = ProfileSelectMenu.profileChosen;
                _transition = FindObjectOfType<TransitionManager>();
        }

        private void Update()
        {
                if (Input.GetKeyUp(KeyCode.Return))
                {
                        PlayerPrefs.SetString(("profile" + _profileSlot + "Name"), nameInputField.text);
                        PlayerPrefs.SetInt("profile" + _profileSlot + "Level", 1);
                        PlayerPrefs.Save();

                        StartCoroutine(_transition.PlayTransition());
                        SceneManager.LoadScene("Level1");
                }
        }
}
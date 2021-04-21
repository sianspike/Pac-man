using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SetName: MonoBehaviour
{
        [SerializeField] public InputField nameInputField;

        private int _profileSlot;

        private void Start()
        {
                _profileSlot = ProfileSelectMenu.profileChosen;
        }

        private void Update()
        {
                if (Input.GetKeyUp(KeyCode.Return))
                {
                        PlayerPrefs.SetString(("profile" + _profileSlot + "Name"), nameInputField.text);
                        PlayerPrefs.SetInt("profile" + _profileSlot + "Level", 1);

                        SceneManager.LoadScene("Level1");
                }
        }
}
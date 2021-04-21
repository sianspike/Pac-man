using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ProfileSelectMenu: MonoBehaviour
{
        [SerializeField] public Text profile1Text;
        [SerializeField] public Text profile2Text;
        [SerializeField] public Text profile3Text;
        [SerializeField] public Text profile1LevelText;
        [SerializeField] public Text profile2LevelText;
        [SerializeField] public Text profile3LevelText;
        [SerializeField] public Text selector;
        [SerializeField] public Text mainMenuText;

        public static int profileChosen;

        private void Start()
        {
            if (PlayerPrefs.HasKey("profile1Name"))
            {
                profile1Text.text = PlayerPrefs.GetString("profile1Name");

                if (PlayerPrefs.HasKey("profile1Level"))
                {
                    profile1LevelText.text = PlayerPrefs.GetInt("profile1Level").ToString();
                }
                
            } else if (PlayerPrefs.HasKey("profile2Name"))
            {
                profile2Text.text = PlayerPrefs.GetString("profile2Name");

                if (PlayerPrefs.HasKey("profile2Level"))
                {
                    profile2LevelText.text = PlayerPrefs.GetInt("profile2Level").ToString();
                }

            } else if (PlayerPrefs.HasKey("profile3Name"))
            {
                profile3Text.text = PlayerPrefs.GetString("profile3Name");

                if (PlayerPrefs.HasKey("profile3Level"))
                {
                    profile3LevelText.text = PlayerPrefs.GetInt("profile3Level").ToString();
                }
            }
        }
        private void Update()
        {
              SelectProfile();  
        }

        private void SelectProfile()
        {
            var selectorPosition = selector.transform.localPosition;

            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                if (selectorPosition.y.Equals((int) mainMenuText.transform.localPosition.y))
                {
                    selector.transform.localPosition = new Vector3(selectorPosition.x,
                        profile1Text.transform.localPosition.y, selectorPosition.z);
                }
                else
                {
                    selector.transform.localPosition =
                        new Vector3(selectorPosition.x, selectorPosition.y - 128, selectorPosition.z);
                }
            } else if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                if (selectorPosition.y.Equals((int) profile1Text.transform.localPosition.y))
                {
                    selector.transform.localPosition = new Vector3(selectorPosition.x,
                        mainMenuText.transform.localPosition.y, selectorPosition.z);
                }
                else
                {
                    selector.transform.localPosition =
                        new Vector3(selectorPosition.x, selectorPosition.y + 128, selectorPosition.z);
                }
            } else if (Input.GetKeyUp(KeyCode.Return))
            {
                if (selectorPosition.y.Equals((int) profile1Text.transform.localPosition.y))
                {
                    profileChosen = 1;
                    
                    if (PlayerPrefs.HasKey("profile1Name"))
                    {
                        SceneManager.LoadScene("Level1");
                    }
                    else
                    {
                        SceneManager.LoadScene("NewProfile");
                    }
                    
                } else if (selectorPosition.y.Equals((int) profile2Text.transform.localPosition.y))
                {
                    profileChosen = 2;
                    
                    if (PlayerPrefs.HasKey("profile2Name"))
                    {
                        SceneManager.LoadScene("Level1");
                    }
                    else
                    {
                        SceneManager.LoadScene("NewProfile");
                    }
                } else if (selectorPosition.y.Equals((int) profile3Text.transform.localPosition.y))
                {
                    profileChosen = 3;
                    
                    if (PlayerPrefs.HasKey("profile3Name"))
                    {
                        SceneManager.LoadScene("Level1");
                    }
                    else
                    {
                        SceneManager.LoadScene("NewProfile");
                    }
                } else if (selectorPosition.y.Equals((int) mainMenuText.transform.localPosition.y))
                {
                    SceneManager.LoadScene("GameMenu");
                }
            }
        }
}
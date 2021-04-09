using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenu: MonoBehaviour
{
        [SerializeField] public Text playText;
        [SerializeField] public Text tutorialText;
        [SerializeField] public Text highScoreText;
        [SerializeField] public Text playerSelector;

        private void Update()
        {
                var playerSelectorPosition = playerSelector.transform.localPosition;
                
                if (Input.GetKeyUp(KeyCode.UpArrow))
                {
                        if (playerSelectorPosition.y.Equals(playText.transform.localPosition.y))
                        {
                                playerSelector.transform.localPosition = new Vector3(playerSelectorPosition.x,
                                        highScoreText.transform.localPosition.y, playerSelectorPosition.z);
                        }
                        else
                        {
                                playerSelector.transform.localPosition = new Vector3(playerSelectorPosition.x,
                                        playerSelectorPosition.y + 55, playerSelectorPosition.y);
                        }

                } else if (Input.GetKeyUp(KeyCode.DownArrow))
                {
                        if (playerSelectorPosition.y.Equals(highScoreText.transform.localPosition.y))
                        {
                                playerSelector.transform.localPosition = new Vector3(playerSelectorPosition.x,
                                        playText.transform.localPosition.y, playerSelectorPosition.z);
                        }
                        else
                        {
                                playerSelector.transform.localPosition = new Vector3(playerSelectorPosition.x,
                                        playerSelectorPosition.y - 55, playerSelector.transform.localPosition.z);
                        }
                   
                        
                } else if (Input.GetKeyUp(KeyCode.Return))
                {
                        SceneManager.LoadScene("Level1");
                }
        }
}
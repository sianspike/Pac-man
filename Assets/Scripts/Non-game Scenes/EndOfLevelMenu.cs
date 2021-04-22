using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndOfLevelMenu: MonoBehaviour
{
        [SerializeField] public Text selector;
        [SerializeField] public Text replayText;
        [SerializeField] public Text continueText;

        private TransitionManager _transition;

        private void Start()
        {
                _transition = FindObjectOfType<TransitionManager>();
        }

        private void Update()
        {
                var selectorPosition = selector.transform.localPosition;
                
                if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.UpArrow))
                {
                        if (selectorPosition.y.Equals(replayText.transform.localPosition.y))
                        {
                                selector.transform.localPosition = new Vector3(selectorPosition.x,
                                        continueText.transform.localPosition.y, selectorPosition.z);
                        }
                        else
                        {
                                selector.transform.localPosition = new Vector3(selectorPosition.x,
                                        replayText.transform.localPosition.y, selectorPosition.z);
                        }
                } else if (Input.GetKeyUp(KeyCode.Return))
                {
                        if (selectorPosition.y.Equals(replayText.transform.localPosition.y))
                        {
                                GameBoard.watchReplaySelected = true;
                        }

                        StartCoroutine(_transition.PlayTransition());
                        SceneManager.LoadScene("Level1");
                }
        }
}
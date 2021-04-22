using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial: MonoBehaviour
{
    private TransitionManager _transition;

    private void Start()
    {
        _transition = FindObjectOfType<TransitionManager>();
    }
    private void Update()
        {
                if (Input.GetKeyUp(KeyCode.Return))
                {
                    StartCoroutine(_transition.PlayTransition());
                    SceneManager.LoadScene("GameMenu");
                }
        }
}
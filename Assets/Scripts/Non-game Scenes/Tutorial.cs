using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial: MonoBehaviour
{
        private void Update()
        {
                if (Input.GetKeyUp(KeyCode.Return))
                {
                    SceneManager.LoadScene("GameMenu");
                }
        }
}
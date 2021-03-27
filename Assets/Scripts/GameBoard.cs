using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    //number of units
    private const int BoardWidth = 27;
    private const int BoardHeight = 30;

    public GameObject[,] board = new GameObject[BoardWidth, BoardHeight];
    
    // Start is called before the first frame update
    void Start()
    {
        var gameObjects = GameObject.FindObjectsOfType(typeof(GameObject));

        foreach (var o in gameObjects)
        {
            var go = (GameObject) o;
            Vector2 position = go.transform.position;

            if (go.name != "pacman")
            {
                board[(int) position.x, (int) position.y] = go;
            }
            else
            {
                Debug.Log("Found pacman at: " + position);
            }
        }
    }
}

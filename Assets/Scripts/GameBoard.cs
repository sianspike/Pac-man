using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    //number of units
    private const int BoardWidth = 31;
    private const int BoardHeight = 31;
    private int _totalPellets = 0;

    public readonly GameObject[,] Board = new GameObject[BoardWidth, BoardHeight];
    public int Score = 0;
    
    // Start is called before the first frame update
    private void Start()
    {
        var gameObjects = GameObject.FindObjectsOfType(typeof(GameObject));

        foreach (var o in gameObjects)
        {
            var go = (GameObject) o;
            Vector2 position = go.transform.position;
            var tile = go.GetComponent<Tile>();

            if (tile != null)
            {
                if (tile.isPellet || tile.isSuperPellet)
                {
                    _totalPellets++;
                }
            }
            
            if (go.name != "pacman" && go.name != "maze" && go.name != "nodes" && go.name != "non_nodes" &&
                go.name != "pellets" && !go.CompareTag("ghost"))
            {
                Board[(int) position.x, (int) position.y] = go;
            }
        }
    }
}

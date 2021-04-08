using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    public static GameBoard Instance;
    
    [SerializeField] public int lives = 3;
    
    //number of units
    private const int BoardWidth = 31;
    private const int BoardHeight = 31;
    private int _totalPellets = 0;
    private Pacman _pacman;
    private GameObject[] _ghostObjects;

    public readonly GameObject[,] Board = new GameObject[BoardWidth, BoardHeight];
    public int score = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            
        } else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }


    // Start is called before the first frame update
    private void Start()
    {
        _pacman = FindObjectOfType<Pacman>();
        _ghostObjects = GameObject.FindGameObjectsWithTag("ghost");
        var gameObjects = FindObjectsOfType(typeof(GameObject));

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
                go.name != "pellets" && !go.CompareTag("ghost") && !go.CompareTag("ghost_home"))
            {
                Board[(int) position.x, (int) position.y] = go;
            }
        }
    }

    public void Restart()
    {
        lives--;
        _pacman.Restart();

        foreach (var ghost in _ghostObjects)
        {
            ghost.transform.GetComponent<Ghost>().Restart();
        }
    }
}

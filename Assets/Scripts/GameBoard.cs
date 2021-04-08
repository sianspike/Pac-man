using System;
using System.Collections;
using System.Collections.Generic;
using Ghosts;
using Pacman;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    public static GameBoard Instance;
    
    [SerializeField] public int lives = 3;
    
    //number of units
    private const int BoardWidth = 31;
    private const int BoardHeight = 31;
    private int _totalPellets = 0;
    private Pacman.Pacman _pacman;
    private GameObject[] _ghostObjects;
    private bool _didStartDeath;
    private Animator _pacmanAnimator;
    private AudioSource _audioSource;
    private PacmanAnimation _pacmanAnimation;
    private SpriteRenderer _pacmanSpriteRenderer;
    private Audio _audio;

    private const float DeathAudioLength = 1.9f;

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
        _pacman = FindObjectOfType<Pacman.Pacman>();
        _ghostObjects = GameObject.FindGameObjectsWithTag("ghost");
        _pacmanAnimator = _pacman.transform.GetComponent<Animator>();
        _audioSource = transform.GetComponent<AudioSource>();
        _pacmanAnimation = _pacman.transform.GetComponent<PacmanAnimation>();
        _pacmanSpriteRenderer = _pacman.transform.GetComponent<SpriteRenderer>();
        _audio = transform.GetComponent<Audio>();
        
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

    private void Restart()
    {
        _didStartDeath = false;
        lives--;
        _pacman.Restart();

        foreach (var ghost in _ghostObjects)
        {
            ghost.transform.GetComponent<Ghost>().Restart();
        }

        _audioSource.clip = _audio.normalBackgroundAudio;
        _audioSource.Play();
    }
    
    public void StartDeath()
    {
        if (!_didStartDeath)
        {
            _didStartDeath = true;
            
            foreach (var ghost in _ghostObjects)
            {
                ghost.transform.GetComponent<Ghost>().canMove = false;
            }

            _pacman.canMove = false;
            _pacmanAnimator.enabled = false;
            
            _audioSource.Stop();

            StartCoroutine(ProcessDeathAfter(2));
        }
    }

    private IEnumerator ProcessDeathAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        foreach (var ghost in _ghostObjects)
        {
            ghost.transform.GetComponent<SpriteRenderer>().enabled = false;
        }

        StartCoroutine(_pacmanAnimation.ProcessDeathAnimation(DeathAudioLength));
    }

    public IEnumerator ProcessRestart(float delay)
    {
        _pacmanSpriteRenderer.enabled = false;
        
        _audioSource.Stop();

        yield return new WaitForSeconds(delay);
        
        Restart();
    }
}

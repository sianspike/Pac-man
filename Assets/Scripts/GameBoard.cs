using System.Collections;
using Ghosts;
using Pacman;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameBoard : MonoBehaviour
{
    public static GameBoard instance;
    
    [SerializeField] public int lives = 3;
    [SerializeField] public Text playerText;
    [SerializeField] public Text readyText;
    [SerializeField] public Text scoreTitleText;
    [SerializeField] public Text scoreText;
    [SerializeField] public Text consumedGhostScoreText;
    [SerializeField] public Image twoLivesImage;
    [SerializeField] public Image threeLivesImage;
    
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
    private PacmanMove _pacmanMove;

    private const float DeathAudioLength = 1.9f;

    public readonly GameObject[,] board = new GameObject[BoardWidth, BoardHeight];
    public int score = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            
        } else if (instance != this)
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
        _pacmanMove = _pacman.transform.GetComponent<PacmanMove>();

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
                go.name != "pellets" && !go.CompareTag("ghost") && !go.CompareTag("ghost_home") && !go.CompareTag("ui"))
            {
                board[(int) position.x, (int) position.y] = go;
            }
        }
        
        StartGame();
    }

    private void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        scoreText.text = score.ToString();

        if (lives == 3)
        {
            threeLivesImage.enabled = true;
            twoLivesImage.enabled = true;

        } else if (lives == 2)
        {
            threeLivesImage.enabled = false;
            twoLivesImage.enabled = true;

        } else if (lives == 1)
        {
            threeLivesImage.enabled = false;
            twoLivesImage.enabled = false;
        }
    }

    private void Restart()
    {
        readyText.enabled = false;
        _didStartDeath = false;
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
            StopAllCoroutines();
            
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
        lives--;
        playerText.enabled = true;
        _pacmanSpriteRenderer.enabled = false;
        
        _audioSource.Stop();

        if (lives == 0)
        {
            readyText.text = "GAME OVER!";
            readyText.color = Color.red;
            readyText.enabled = true;

            StartCoroutine(ProcessGameOver(2));
        }
        else
        { 
            readyText.enabled = true;
            
            yield return new WaitForSeconds(delay);

            StartCoroutine(ShowSpritesAndRestartAfter(1));
        }
    }

    private void StartGame()
    {
        foreach (var ghost in _ghostObjects)
        {
            ghost.transform.GetComponent<Ghost>().canMove = false;
            ghost.transform.GetComponent<SpriteRenderer>().enabled = false;
        }

        _pacman.canMove = false;
        _pacmanSpriteRenderer.enabled = false;

        StartCoroutine(ShowSpritesAfter(2.25f));
    }

    private IEnumerator ShowSpritesAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        foreach (var ghost in _ghostObjects)
        {
            ghost.transform.GetComponent<SpriteRenderer>().enabled = true;
        }
        
        _pacmanSpriteRenderer.enabled = true;
        playerText.enabled = false;

        StartCoroutine(StartGameAfter(2));
    }

    private IEnumerator StartGameAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        foreach (var ghost in _ghostObjects)
        {
            ghost.transform.GetComponent<Ghost>().canMove = true;
        }

        _pacman.canMove = true;
        readyText.enabled = false;
        _audioSource.clip = _audio.normalBackgroundAudio;
        _audioSource.Play();
    }

    private IEnumerator ShowSpritesAndRestartAfter(float delay)
    {
        playerText.enabled = false;
        
        foreach (var ghost in _ghostObjects)
        {
            ghost.transform.GetComponent<SpriteRenderer>().enabled = true;
            ghost.transform.GetComponent<GhostMove>().MoveToStartingPosition();
        }
        
        _pacmanSpriteRenderer.enabled = true;
        _pacmanMove.MoveToStartingPosition();

        yield return new WaitForSeconds(delay);
        
        Restart();
    }

    private IEnumerator ProcessGameOver(float delay)
    {
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene("GameMenu");
    }
}

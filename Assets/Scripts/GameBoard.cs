using System.Collections;
using System.Diagnostics;
using Ghosts;
using Pacman;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

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
    private Pacman.Pacman _pacman;
    private GameObject[] _ghostObjects;
    private bool _didStartDeath;
    private Animator _pacmanAnimator;
    private AudioSource _audioSource;
    private PacmanAnimation _pacmanAnimation;
    private SpriteRenderer _pacmanSpriteRenderer;
    private Audio _audio;
    private PacmanMove _pacmanMove;
    private PacmanConsume _pacmanConsume;
    private static int _currentLevel = 1;
    private bool _shouldBlink = false;
    private float _blinkIntervalTime = 0.1f;
    private float _blinkIntervalTimer = 0;
    private SpriteRenderer _mazeSpriteRenderer;
    private bool _didIncrementLevel = false;

    private const float DeathAudioLength = 1.9f;

    public readonly GameObject[,] board = new GameObject[BoardWidth, BoardHeight];
    public static int score = 0;
    public int totalPellets = 0;

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
        _pacmanConsume = _pacman.GetComponent<PacmanConsume>();
        _mazeSpriteRenderer = GameObject.Find("maze").GetComponent<SpriteRenderer>();

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
                    totalPellets++;
                }
            }

            if (go.name != "pacman" && go.name != "maze" && go.name != "nodes" && go.name != "non_nodes" &&
                go.name != "pellets" && !go.CompareTag("ghost") && !go.CompareTag("ghost_home") && !go.CompareTag("ui"))
            {
                board[(int) position.x, (int) position.y] = go;
            }
        }

        if (_currentLevel == 1)
        {
            _audioSource.Play();
            Pacman.Pacman.pacmanSpeed = 6;
            Ghost.ghostSpeed = 5.9f;
        }
        
        StartGame();
    }

    private void Update()
    {
        UpdateUI();
        _pacmanConsume.CheckPelletsConsumed();
        BoardShouldBlink();
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
            score = 0;

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

    public void PlayerWon()
    {
        if (!_didIncrementLevel)
        {
            _didIncrementLevel = true;
            _currentLevel++;
        }

        StartCoroutine(ProcessWin(2));
    }

    private IEnumerator ProcessWin(float delay)
    {
        _pacman.canMove = false;
        _pacmanAnimation.enabled = false;
        
        _audioSource.Stop();
        
        foreach (var ghost in _ghostObjects)
        {
            ghost.transform.GetComponent<Ghost>().canMove = false;
            ghost.transform.GetComponent<Animator>().enabled = false;
        }

        yield return new WaitForSeconds(delay);

        StartCoroutine(BlinkBoard(2));
    }

    private IEnumerator BlinkBoard(float delay)
    {
        _pacmanSpriteRenderer.enabled = false;
        
        foreach (var ghost in _ghostObjects)
        {
            ghost.transform.GetComponent<SpriteRenderer>().enabled = false;
        }

        _shouldBlink = true;

        yield return new WaitForSeconds(delay);

        _shouldBlink = false;
        
        _didIncrementLevel = false;
        SetDifficultyForLevel(_currentLevel);
        EndOfLevelMenu();
    }

    private void BoardShouldBlink()
    {
        if (_shouldBlink)
        {
            if (_blinkIntervalTimer < _blinkIntervalTime)
            {
                _blinkIntervalTimer += Time.deltaTime;
            }
            else
            {
                _blinkIntervalTimer = 0;

                if (_mazeSpriteRenderer.enabled)
                {
                    _mazeSpriteRenderer.enabled = false;
                }
                else
                {
                    _mazeSpriteRenderer.enabled = true;
                }
            }
        }
    }

    private void EndOfLevelMenu()
    {
        SceneManager.LoadScene("EndOfLevel");
    }

    private void SetDifficultyForLevel(int level)
    {
        switch (level)
        {
            case 2:
                Pacman.Pacman.pacmanSpeed = 7;
                GhostMode.scatterModeTimer1 = 7;
                GhostMode.scatterModeTimer2 = 7;
                GhostMode.scatterModeTimer3 = 5;
                GhostMode.scatterModeTimer4 = 1;
                GhostMode.chaseModeTimer1 = 20;
                GhostMode.chaseModeTimer2 = 20;
                GhostMode.chaseModeTimer3 = 1033;
                GhostMode.frightenedModeDuration = 9;
                Ghost.startBlinkingAt = 6;
                Ghost.pinkyReleaseTimer = 4;
                Ghost.inkyReleaseTimer = 12;
                Ghost.clydeReleaseTimer = 18;
                Ghost.ghostSpeed = 6.9f;
                GhostMode.normalSpeed = 6.9f;
                GhostMode.frightenedSpeed = 3.9f;
                GhostMode.consumedSpeed = 18f;

                break;

            case 3:
                Pacman.Pacman.pacmanSpeed = 8;
                GhostMode.scatterModeTimer1 = 7;
                GhostMode.scatterModeTimer2 = 7;
                GhostMode.scatterModeTimer3 = 5;
                GhostMode.scatterModeTimer4 = 1;
                GhostMode.chaseModeTimer1 = 20;
                GhostMode.chaseModeTimer2 = 20;
                GhostMode.chaseModeTimer3 = 1033;
                GhostMode.frightenedModeDuration = 8;
                Ghost.startBlinkingAt = 5;
                Ghost.pinkyReleaseTimer = 3;
                Ghost.inkyReleaseTimer = 10;
                Ghost.clydeReleaseTimer = 15;
                Ghost.ghostSpeed = 7.9f;
                GhostMode.normalSpeed = 7.9f;
                GhostMode.frightenedSpeed = 4.9f;
                GhostMode.consumedSpeed = 20f;

                break;

            case 4:
                Pacman.Pacman.pacmanSpeed = 9;
                GhostMode.scatterModeTimer1 = 7;
                GhostMode.scatterModeTimer2 = 7;
                GhostMode.scatterModeTimer3 = 5;
                GhostMode.scatterModeTimer4 = 1;
                GhostMode.chaseModeTimer1 = 20;
                GhostMode.chaseModeTimer2 = 20;
                GhostMode.chaseModeTimer3 = 1033;
                GhostMode.frightenedModeDuration = 9;
                Ghost.startBlinkingAt = 4;
                Ghost.pinkyReleaseTimer = 2;
                Ghost.inkyReleaseTimer = 8;
                Ghost.clydeReleaseTimer = 13;
                Ghost.ghostSpeed = 8.9f;
                GhostMode.normalSpeed = 8.9f;
                GhostMode.frightenedSpeed = 5.9f;
                GhostMode.consumedSpeed = 22f;

                break;
        }

        if (_currentLevel > 4) {
            
                Pacman.Pacman.pacmanSpeed = 10;
                GhostMode.scatterModeTimer1 = 5;
                GhostMode.scatterModeTimer2 = 5;
                GhostMode.scatterModeTimer3 = 5;
                GhostMode.scatterModeTimer4 = 1;
                GhostMode.chaseModeTimer1 = 20;
                GhostMode.chaseModeTimer2 = 20;
                GhostMode.chaseModeTimer3 = 1037;
                GhostMode.frightenedModeDuration = 6;
                Ghost.startBlinkingAt = 3;
                Ghost.pinkyReleaseTimer = 2;
                Ghost.inkyReleaseTimer = 6;
                Ghost.clydeReleaseTimer = 10;
                Ghost.ghostSpeed = 9.9f;
                GhostMode.normalSpeed = 9.9f;
                GhostMode.frightenedSpeed = 6.9f;
                GhostMode.consumedSpeed = 24f;
        }
    }
}

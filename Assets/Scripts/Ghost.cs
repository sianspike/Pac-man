using ScriptResources;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [SerializeField] public GhostType ghostType = GhostType.Blinky;
    [SerializeField] public Node homeNode;
    [SerializeField] public bool isInGhostHouse;
    [SerializeField] public RuntimeAnimatorController ghostUp;
    [SerializeField] public RuntimeAnimatorController ghostDown;
    [SerializeField] public RuntimeAnimatorController ghostLeft;
    [SerializeField] public RuntimeAnimatorController ghostRight;
    [SerializeField] public RuntimeAnimatorController ghostFrightened;
    [SerializeField] public RuntimeAnimatorController ghostWhite;
    [SerializeField] public Sprite eyesUp;
    [SerializeField] public Sprite eyesDown;
    [SerializeField] public Sprite eyesLeft;
    [SerializeField] public Sprite eyesRight;
    [SerializeField] public Node ghostHouse;

    public float ghostReleaseTimer;
    public int pinkyReleaseTimer = 5;
    public int inkyReleaseTimer = 14;
    public int clydeReleaseTimer = 21;
    public int startBlinkingAt = 7;
    public float blinkTimer;
    public bool ghostIsWhite;
    public Node startingPosition;
    
    private GhostMove _ghostMovement;
    public Vector2 direction = Vector2.zero;
    private GhostMode _mode;
    private Collision _collision;
    private GhostAnimation _ghostAnimation;
    private GhostOrientation _ghostOrientation;

    private void Start()
    {
        _ghostMovement = GetComponent<GhostMove>();
        _mode = GetComponent<GhostMode>();
        _collision = GetComponent<Collision>();
        _ghostAnimation = GetComponent<GhostAnimation>();
        _ghostOrientation = GetComponent<GhostOrientation>();
    }

    private void Update()
    {
        _ghostOrientation.UpdateOrientation(direction);
        _mode.ModeUpdate();
        direction = _ghostMovement.MoveSprite();
        _ghostAnimation.UpdateAnimation(direction);
        _ghostMovement.ReleaseGhosts();
        _collision.CheckCollision(_mode);
        IsInGhostHouse();
    }

    private void IsInGhostHouse()
    {
        if (_mode.currentMode == Mode.Consumed)
        {
            var tile = _ghostMovement.GetTileAtPosition(transform.position);

            if (tile != null)
            {
                var tileComponent = tile.transform.GetComponent<Tile>();

                if (tileComponent != null)
                {
                    if (tileComponent.isGhostHouse)
                    {
                        _ghostMovement.speed = _mode.normalSpeed;

                        var node = _ghostMovement.GetNodeAtPosition(transform.position);

                        if (node != null)
                        {
                            _ghostMovement.currentNode = node;
                            direction = Vector2.up;
                            _ghostAnimation.UpdateAnimation(direction);
                            _ghostMovement.targetNode = _ghostMovement.currentNode.neighbours[0];
                            _ghostMovement.previousNode = _ghostMovement.currentNode;
                            _mode.currentMode = Mode.Chase;
                        }
                    }
                }
            }
        }
    }

    public void Restart()
    {
        transform.position = startingPosition.transform.position;
        ghostReleaseTimer = 0;
        _mode.modeChangeIteration = 1;
        _mode.modeChangeTimer = 0;

        if (transform.name != "blinky")
        {
            isInGhostHouse = true;
        }

        _ghostMovement.currentNode = startingPosition;

        if (isInGhostHouse)
        {
            direction = Vector2.up;
            _ghostMovement.targetNode = _ghostMovement.currentNode.neighbours[0];
        }
        else
        {
            direction = Vector2.right;
            _ghostMovement.targetNode = _ghostMovement.ChooseNextNode();
        }

        _ghostMovement.previousNode = _ghostMovement.currentNode;
        _ghostAnimation.UpdateAnimation(direction);
    }
}
using ScriptResources;
using UnityEngine;

namespace Ghosts
{
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
        public bool canMove = true;
    
        private GhostMove _ghostMovement;
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
            if (canMove)
            {
                _mode.ModeUpdate();
                _ghostMovement.direction = _ghostMovement.MoveSprite();
                _ghostOrientation.UpdateOrientation(_ghostMovement.direction);
                _ghostAnimation.UpdateAnimation(_ghostMovement.direction);
                _ghostMovement.ReleaseGhosts();
                _collision.CheckCollision(_mode);
                IsInGhostHouse();
            }
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
                                _ghostMovement.direction = Vector2.up;
                                _ghostAnimation.UpdateAnimation(_ghostMovement.direction);
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
            canMove = true;
            _mode.currentMode = Mode.Scatter;
            _ghostMovement.speed = _mode.normalSpeed;
            _mode.previousSpeed = 0;
            ghostReleaseTimer = 0;
            _mode.modeChangeIteration = 1;
            _mode.modeChangeTimer = 0;

            if (isInGhostHouse)
            {
                _ghostMovement.direction = Vector2.up;
                _ghostMovement.targetNode = _ghostMovement.currentNode.neighbours[0];
            }
            else
            {
                _ghostMovement.direction = Vector2.right;
                _ghostMovement.targetNode = _ghostMovement.ChooseNextNode();
            }
        }
    }
}
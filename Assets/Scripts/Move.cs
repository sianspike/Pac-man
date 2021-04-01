using JetBrains.Annotations;
using UnityEngine;

public class PacmanMove : MonoBehaviour 
{
    [SerializeField] public float speed = 4.0f;

    private Vector2 _direction = Vector2.zero;
    private Vector2 _nextDirection;
    private Node _previousNode, _currentNode, _targetNode;
    private GameBoard _gameBoard;
    private Orientation _pacmanOrientation;
    private PacmanAnimation _animation;
    private Consume _consumePellet;

    private void Start()
    {
        _gameBoard = GameObject.Find("game").GetComponent<GameBoard>();
        _pacmanOrientation = GetComponent<Orientation>();
        _animation = GetComponent<PacmanAnimation>();
        _consumePellet = GetComponent<Consume>();
        
        //pacman position
        var node = GetNodeAtPosition(transform.localPosition);

        if (node == null) return;
        
        _currentNode = node;
        
        Debug.Log(_currentNode);

        _direction = Vector2.right;
        
        ChangePosition(_direction);
    }

    private void Update()
    {
        CheckInput();
        Move();
        _pacmanOrientation.UpdateOrientation(_direction);
        _animation.UpdateAnimation(_direction);
        _consumePellet.ConsumePellet(_gameBoard);
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangePosition(Vector2.left);
        } 
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangePosition(Vector2.right);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangePosition(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChangePosition(Vector2.down);
        }
    }

    private void Move()
    {
        if (_targetNode == _currentNode || ReferenceEquals(_targetNode, null)) return;

        //if pacman wants to move in opposite direction before node is reached
        if (_nextDirection == _direction * -1)
        {
            _direction *= -1;

            var temporaryNode = _targetNode;
            
            _targetNode = _previousNode;
            _previousNode = temporaryNode;
        }
        
        if (OvershotTarget())
        {
            _currentNode = _targetNode;
            transform.localPosition = _currentNode.transform.position;

            var moveToNode = CanMove(_nextDirection);

            if (!ReferenceEquals(moveToNode, null))
            {
                _direction = _nextDirection;
            }

            if (ReferenceEquals(moveToNode, null))
            {
                moveToNode = CanMove(_direction);
            }

            if (!ReferenceEquals(moveToNode, null))
            {
                _targetNode = moveToNode;
                _previousNode = _currentNode;
                _currentNode = null;
            }
            else
            {
                _direction = Vector2.zero;
            }
        }
        else
        {
            transform.localPosition += (Vector3) (_direction * speed) * Time.deltaTime;
        }
    }
    
    private Node GetNodeAtPosition(Vector2 position)
    {
        var tile = _gameBoard.Board[(int) position.x, (int) position.y];

        return tile != null ? tile.GetComponent<Node>() : null;
    }
    
    private Node CanMove(Vector2 direction)
    {
        Node moveToNode = null;

        //iterate over neighbours of pellet
        for (var i = 0; i < _currentNode.neighbours.Length; i++)
        {
            //check if current direction of pacman is valid
            if (_currentNode.validDirections[i] != direction) continue;
            
            moveToNode = _currentNode.neighbours[i];
            
            break;
        }

        return moveToNode;
    }
    
    private void MoveToNode(Vector2 direction)
    {
        var moveToNode = CanMove(direction);

        if (moveToNode == null) return;
        
        //change pacman's position if he is able to move
        transform.localPosition = moveToNode.transform.position;
        _currentNode = moveToNode;
    }
    
    private void ChangePosition(Vector2 targetDirection)
    {
        if (targetDirection != _direction)
        {
            _nextDirection = targetDirection;
        }

        if (ReferenceEquals(_currentNode, null)) return;
        
        var moveToNode = CanMove(targetDirection);

        if (ReferenceEquals(moveToNode, null)) return;
            
        _direction = targetDirection;
        _targetNode = moveToNode;
        _previousNode = _currentNode;
        _currentNode = null;
    }
    
    private float LengthFromNode(Vector2 targetPosition)
    {
        var length = targetPosition - (Vector2) _previousNode.transform.position;

        return length.sqrMagnitude;
    }

    private bool OvershotTarget()
    {
        var nodeToTarget = LengthFromNode(_targetNode.transform.position);
        var nodeToSelf = LengthFromNode(transform.localPosition);

        return nodeToSelf > nodeToTarget;
    }
}
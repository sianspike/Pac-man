using System;
using JetBrains.Annotations;
using UnityEngine;

public class Move : MonoBehaviour 
{
    [SerializeField] public float speed = 4.0f;
    [SerializeField] public GhostType ghostType = GhostType.Blinky;
    
    private Vector2 _direction = Vector2.zero;
    private Vector2 _nextDirection;
    private Node _previousNode, _targetNode, _currentNode;
    private GameBoard _gameBoard;
    private Pacman _pacman;
    public bool isInGhostHouse = false;

    private void Start()
    {
        _gameBoard = GameObject.Find("game").GetComponent<GameBoard>();
        _pacman = GameObject.FindGameObjectWithTag("pacman").GetComponent<Pacman>();

        var node = GetNodeAtPosition(transform.position, _gameBoard);

        if (node == null) return;
        
        _currentNode = node;
        _previousNode = _currentNode;
        
        if (isInGhostHouse)
        {
            _direction = Vector2.up;
            _targetNode = _currentNode.neighbours[0];
        }
        else
        {
            _direction = Vector2.right;
            _targetNode = ChooseNextNode();
        }
    }

    public void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangePacmanPosition(Vector2.left);
        } 
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangePacmanPosition(Vector2.right);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangePacmanPosition(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChangePacmanPosition(Vector2.down);
        }
    }
    
    public Vector2 MovePacman()
    {
        if (_targetNode == _currentNode || ReferenceEquals(_targetNode, null)) return default;

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
            
            var moveToNode = PacmanCanMove(_nextDirection);
            
            if (!ReferenceEquals(moveToNode, null))
            {
                _direction = _nextDirection;
            }
            
            if (ReferenceEquals(moveToNode, null))
            {
                moveToNode = PacmanCanMove(_direction);
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

        return _direction;
    }

    public static Node GetNodeAtPosition(Vector2 position, GameBoard gameBoard)
    {
        var tile = gameBoard.Board[(int) position.x, (int) position.y];

        return tile != null ? tile.GetComponent<Node>() : null;
    }
    
    private Node PacmanCanMove(Vector2 direction)
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
    
    public void ChangePacmanPosition(Vector2 targetDirection)
    {
        if (targetDirection != _direction)
        {
            _nextDirection = targetDirection;
        }

        if (!ReferenceEquals(_currentNode, null))
        {
            var moveToNode = PacmanCanMove(targetDirection);

            if (!ReferenceEquals(moveToNode, null))
            {
                _direction = targetDirection;
                _targetNode = moveToNode;
                _previousNode = _currentNode;
                _currentNode = null;
            }
        }
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
    
    public Vector2 MoveGhost()
    {
        if (_targetNode == _currentNode || ReferenceEquals(_targetNode, null)) return default;
        
        if (OvershotTarget())
        {
            _currentNode = _targetNode;
            transform.localPosition = _currentNode.transform.position;
            _targetNode = ChooseNextNode();
            _previousNode = _currentNode;
            _currentNode = null;
        }
        else
        {
            transform.localPosition += (Vector3) _direction * (speed * Time.deltaTime); 
        }
        
        return _direction;
    }
    
    private Node ChooseNextNode()
    {
        Node moveToNode = null;
        var targetTile = GetTargetTile();
        var foundNeighbours = new Node[4];
        var foundNeighboursDirection = new Vector2[4];
        var nodeCounter = 0;

        for (var i = 0; i < _currentNode.neighbours.Length; i++)
        {
            if (_currentNode.validDirections[i] == _direction * -1) continue;
            
            foundNeighbours[nodeCounter] = _currentNode.neighbours[i];
            foundNeighboursDirection[nodeCounter] = _currentNode.validDirections[i];
            nodeCounter++;
        }

        if (foundNeighbours.Length == 1)
        {
            moveToNode = foundNeighbours[0];
            _direction = foundNeighboursDirection[0];
        }

        if (foundNeighbours.Length > 1)
        {
            var leastDistance = 10000f;
            
            for (var i = 0; i < foundNeighbours.Length; i++)
            {
                if (foundNeighboursDirection[i] == Vector2.zero) continue;
                
                var distance = GetDistance(foundNeighbours[i].transform.localPosition, 
                    targetTile);

                if (!(distance < leastDistance)) continue;
                    
                leastDistance = distance;
                moveToNode = foundNeighbours[i];
                _direction = foundNeighboursDirection[i];
            }
        }

        return moveToNode;
    }
    
    private float GetDistance(Vector2 positionA, Vector2 positionB)
    {
        var xDistance = positionA.x - positionB.x;
        var yDistance = positionA.y - positionB.y;
        var distance = Mathf.Sqrt(xDistance * xDistance + yDistance * yDistance);

        return distance;
    }

    private Vector2 GetBlinkyTargetTile()
    {
        Vector2 pacmanPosition = _pacman.transform.localPosition;
        var targetTile = new Vector2(Mathf.RoundToInt(pacmanPosition.x), Mathf.RoundToInt(pacmanPosition.y));

        return targetTile;
    }

    private Vector2 GetPinkyTargetTile()
    {
        //4 tiles ahead of pacman
        Vector2 pacmanPosition = _pacman.transform.localPosition;
        //could be wrong
        Vector2 pacmanOrientation = _pacman.pacmanOrientation.transform.position;
        var pacmanPositionX = Mathf.RoundToInt(pacmanPosition.x);
        var pacmanPositionY = Mathf.RoundToInt(pacmanPosition.y);
        var pacmanTile = new Vector2(pacmanPositionX, pacmanPositionY);
        var targetTile = pacmanTile + (4 * pacmanOrientation);

        return targetTile;
    }

    private Vector2 GetTargetTile()
    {
        var targetTile = Vector2.zero;

        if (ghostType == GhostType.Blinky)
        {
            targetTile = GetBlinkyTargetTile();
            
        } else if (ghostType == GhostType.Pinky)
        {
            targetTile = GetPinkyTargetTile();
        }

        return targetTile;
    }
}
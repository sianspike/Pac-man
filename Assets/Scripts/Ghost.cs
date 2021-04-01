using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [SerializeField] public float speed = 3.9f;

    public Node startingPosition;

    private int _scatterModeTimer1 = 7;
    private int _chaseModeTimer1 = 20;
    private int _scatterModeTimer2 = 7;
    private int _chaseModeTimer2 = 20;
    private int _scatterModeTimer3 = 5;
    private int _chaseModeTimer3 = 20;
    private int _scatterModeTimer4 = 5;
    private int _chaseModeTimer4 = 20;
    private int _modeChangeIteration = 1;
    private float _modeChangeTimer = 0;
    private Mode _currentMode = Mode.Scatter;
    private Mode _previousMode;
    private GameObject _pacman;
    private Node _currentNode, _targetNode, _previousNode;
    private Vector2 _direction, _nextDirection;
    private GameBoard _gameBoard;

    private void Start()
    {
        _gameBoard = GameObject.Find("game").GetComponent<GameBoard>();
    }

    private Node GetNodeAtPosition(Vector2 position)
    {
        var tile = _gameBoard.Board[(int) position.x, (int) position.y];
        var node = tile.GetComponent<Node>();

        if (tile == null) return null;
        
        return node != null ? node : null;
    }

    private void ChangeMode(Mode mode)
    {
        _currentMode = mode;
    }

    private void ModeUpdate()
    {
        if (_currentMode == Mode.Frightened) return;
        
        _modeChangeTimer += Time.deltaTime;

        if (_modeChangeIteration != 1) return;
            
        if (_currentMode == Mode.Scatter && _modeChangeTimer > _scatterModeTimer1)
        {
            ChangeMode(Mode.Chase);
            _modeChangeTimer = 0;
        }

        if (_currentMode != Mode.Chase || !(_modeChangeTimer > _chaseModeTimer1)) return;
                
        _modeChangeIteration = 2;
        ChangeMode(Mode.Scatter);
        _modeChangeTimer = 0;
    }
}

public enum Mode
{
    Chase,
    Scatter,
    Frightened
}
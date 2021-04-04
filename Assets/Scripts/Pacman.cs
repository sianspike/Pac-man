using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pacman : MonoBehaviour
{
    private Vector2 _direction = Vector2.zero;
    private GameBoard _gameBoard;
    private Orientation _pacmanOrientation;
    private PacmanAnimation _animation;
    private Consume _consumePellet;
    private Move _pacmanMovement;

    private void Start()
    {
        _gameBoard = GameObject.Find("game").GetComponent<GameBoard>();
        _pacmanOrientation = GetComponent<Orientation>();
        _animation = GetComponent<PacmanAnimation>();
        _consumePellet = GetComponent<Consume>();
        _pacmanMovement = GetComponent<Move>();
        
        _pacmanMovement.ChangePacmanPosition(_direction);
    }

    private void Update()
    {
        _direction = _pacmanMovement.MovePacman();
        _pacmanOrientation.UpdateOrientation(_direction);
        _animation.UpdateAnimation(_direction);
        _consumePellet.ConsumePellet(_gameBoard);
        
        _pacmanMovement.CheckInput();
    }
}

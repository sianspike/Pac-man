using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Pacman : MonoBehaviour
{
    private Vector2 _direction = Vector2.zero;
    private GameBoard _gameBoard;
    public Orientation pacmanOrientation;
    private PacmanAnimation _animation;
    private Consume _consumePellet;
    private Move _pacmanMovement;

    private void Start()
    {
        _gameBoard = GameObject.Find("game").GetComponent<GameBoard>();
        pacmanOrientation = GetComponent<Orientation>();
        _animation = GetComponent<PacmanAnimation>();
        _consumePellet = GetComponent<Consume>();
        _pacmanMovement = GetComponent<Move>();
        
        _pacmanMovement.ChangePacmanPosition(_direction);
    }

    private void Update()
    {
        _direction = _pacmanMovement.MovePacman();
        pacmanOrientation.UpdateOrientation(_direction);
        _animation.UpdateAnimation(_direction);
        _consumePellet.ConsumePellet(_gameBoard);
        
        _pacmanMovement.CheckInput();
    }
}

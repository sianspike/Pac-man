using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    private const int ScatterModeTimer1 = 7;
    private const int ChaseModeTimer1 = 20;
    private const int ScatterModeTimer2 = 7;
    private const int ChaseModeTimer2 = 20;
    private const int ScatterModeTimer3 = 5;
    private const int ChaseModeTimer3 = 20;
    private const int ScatterModeTimer4 = 5;
    private int _modeChangeIteration = 1;
    private float _modeChangeTimer;
    private Mode _currentMode = Mode.Scatter;
    private Move _ghostMovement;
    private GameObject _pacman;
    private GameBoard _gameBoard;
    private Node _targetNode;

    private void Start()
    {
        _pacman = GameObject.FindGameObjectWithTag("pacman");
        _gameBoard = GameObject.Find("game").GetComponent<GameBoard>();
        _ghostMovement = GetComponent<Move>();

        Vector2 pacmanPosition = _pacman.transform.position;
        var targetTile = new Vector2(Mathf.RoundToInt(pacmanPosition.x), Mathf.RoundToInt(pacmanPosition.y));
        
        _targetNode = Move.GetNodeAtPosition(targetTile, _gameBoard);
        
        _ghostMovement.SetTargetNode(_targetNode);
    }

    private void Update()
    {
        ModeUpdate();
        _ghostMovement.MoveGhost();
    }

    private void ChangeMode(Mode mode)
    {
        _currentMode = mode;
    }

    private void ModeUpdate()
    {
        if (_currentMode != Mode.Frightened)
        {
            _modeChangeTimer += Time.deltaTime;

            switch (_modeChangeIteration)
            {
                case 1:
                {
                    if (_currentMode == Mode.Scatter && _modeChangeTimer > ScatterModeTimer1)
                    {
                        ChangeMode(Mode.Chase);
                        _modeChangeTimer = 0;
                    }

                    if (_currentMode != Mode.Chase || !(_modeChangeTimer > ChaseModeTimer1)) return;

                    _modeChangeIteration = 2;
                    ChangeMode(Mode.Scatter);
                    _modeChangeTimer = 0;
                    break;
                }
                case 2:
                {
                    if (_currentMode == Mode.Scatter && _modeChangeTimer > ScatterModeTimer2)
                    {
                        ChangeMode(Mode.Chase);
                        _modeChangeTimer = 0;
                    }

                    if (_currentMode != Mode.Chase || !(_modeChangeTimer > ChaseModeTimer2)) return;
                
                    _modeChangeIteration = 3;
                    ChangeMode(Mode.Scatter);
                    _modeChangeTimer = 0;
                    break;
                }
                case 3:
                {
                    if (_currentMode == Mode.Scatter && _modeChangeTimer > ScatterModeTimer3)
                    {
                        ChangeMode(Mode.Chase);
                        _modeChangeTimer = 0;
                    }

                    if (_currentMode != Mode.Chase || !(_modeChangeTimer > ChaseModeTimer3)) return;
                
                    _modeChangeIteration = 4;
                    ChangeMode(Mode.Scatter);
                    _modeChangeTimer = 0;
                    break;
                }
                case 4 when _currentMode != Mode.Scatter || !(_modeChangeTimer > ScatterModeTimer4):
                    return;
                
                case 4:
                    ChangeMode(Mode.Chase);
                    _modeChangeTimer = 0;
                    break;
            }
        }
        else if (_currentMode == Mode.Frightened)
        {

        }
    }
}

public enum Mode
{
    Chase,
    Scatter,
    Frightened
}
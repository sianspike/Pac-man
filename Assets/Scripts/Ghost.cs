using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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
    [SerializeField] public float frightenedSpeed = 2.9f;
    
    public Mode currentMode = Mode.Scatter;
    public float ghostReleaseTimer = 0;
    public int pinkyReleaseTimer = 5;
    public int inkyReleaseTimer = 14;
    public int clydeReleaseTimer = 21;
    public int startBlinkingAt = 7;
    public float frightenedModeTimer;
    public float blinkTimer;
    public bool ghostIsWhite;

    private const int ScatterModeTimer1 = 7;
    private const int ChaseModeTimer1 = 20;
    private const int ScatterModeTimer2 = 7;
    private const int ChaseModeTimer2 = 20;
    private const int ScatterModeTimer3 = 5;
    private const int ChaseModeTimer3 = 20;
    private const int ScatterModeTimer4 = 5;
    private int _modeChangeIteration = 1;
    private float _modeChangeTimer;
    private Move _ghostMovement;
    private Orientation _ghostOrientation;
    private Vector2 _direction = Vector2.zero;
    private float _previousSpeed;
    private Mode _previousMode;
    private const int FrightenedModeDuration = 10;

    private void Start()
    {
        _ghostMovement = GetComponent<Move>();
        _ghostOrientation = GetComponent<Orientation>();
    }

    private void Update()
    {
        ModeUpdate();
        _direction = _ghostMovement.MoveGhost();
        _ghostOrientation.UpdateOrientation(_direction);
        _ghostMovement.ReleaseGhosts();
    }

    private void ChangeMode(Mode mode)
    {
        if (currentMode == Mode.Frightened)
        {
            _ghostMovement.speed = _previousSpeed;
        }

        if (mode == Mode.Frightened)
        {
            _previousSpeed = _ghostMovement.speed;
            _ghostMovement.speed = frightenedSpeed;
        }

        _previousMode = currentMode;
        currentMode = mode;
    }

    private void ModeUpdate()
    {
        if (currentMode != Mode.Frightened)
        {
            _modeChangeTimer += Time.deltaTime;

            switch (_modeChangeIteration)
            {
                case 1:
                {
                    if (currentMode == Mode.Scatter && _modeChangeTimer > ScatterModeTimer1)
                    {
                        ChangeMode(Mode.Chase);
                        _modeChangeTimer = 0;
                    }

                    if (currentMode != Mode.Chase || !(_modeChangeTimer > ChaseModeTimer1)) return;

                    _modeChangeIteration = 2;
                    ChangeMode(Mode.Scatter);
                    _modeChangeTimer = 0;
                    break;
                }
                case 2:
                {
                    if (currentMode == Mode.Scatter && _modeChangeTimer > ScatterModeTimer2)
                    {
                        ChangeMode(Mode.Chase);
                        _modeChangeTimer = 0;
                    }

                    if (currentMode != Mode.Chase || !(_modeChangeTimer > ChaseModeTimer2)) return;
                
                    _modeChangeIteration = 3;
                    ChangeMode(Mode.Scatter);
                    _modeChangeTimer = 0;
                    break;
                }
                case 3:
                {
                    if (currentMode == Mode.Scatter && _modeChangeTimer > ScatterModeTimer3)
                    {
                        ChangeMode(Mode.Chase);
                        _modeChangeTimer = 0;
                    }

                    if (currentMode != Mode.Chase || !(_modeChangeTimer > ChaseModeTimer3)) return;
                
                    _modeChangeIteration = 4;
                    ChangeMode(Mode.Scatter);
                    _modeChangeTimer = 0;
                    break;
                }
                case 4 when currentMode != Mode.Scatter || !(_modeChangeTimer > ScatterModeTimer4):
                    return;
                
                case 4:
                    ChangeMode(Mode.Chase);
                    _modeChangeTimer = 0;
                    break;
            }
        }
        else if (currentMode == Mode.Frightened)
        {
            frightenedModeTimer += Time.deltaTime;

            if (frightenedModeTimer >= FrightenedModeDuration)
            {
                frightenedModeTimer = 0;
                ChangeMode(_previousMode);
            }
        }
    }

    public void StartFrightenedMode()
    {
        ChangeMode(Mode.Frightened);
    }
}

public enum Mode
{
    Chase,
    Scatter,
    Frightened
}

public enum GhostType
{
    Blinky,
    Pinky,
    Inky,
    Clyde
}
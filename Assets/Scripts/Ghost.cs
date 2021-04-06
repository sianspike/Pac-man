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
    
    public float ghostReleaseTimer = 0;
    public int pinkyReleaseTimer = 5;
    public int inkyReleaseTimer = 14;
    public int clydeReleaseTimer = 21;
    public int startBlinkingAt = 7;
    public float blinkTimer;
    public bool ghostIsWhite;
    
    private Move _ghostMovement;
    private Orientation _ghostOrientation;
    private Vector2 _direction = Vector2.zero;
    private GhostMode _mode;

    private void Start()
    {
        _ghostMovement = GetComponent<Move>();
        _ghostOrientation = GetComponent<Orientation>();
        _mode = GetComponent<GhostMode>();
    }

    private void Update()
    {
        _mode.ModeUpdate();
        _direction = _ghostMovement.MoveGhost();
        _ghostOrientation.UpdateOrientation(_direction);
        _ghostMovement.ReleaseGhosts();
    }
}

public enum GhostType
{
    Blinky,
    Pinky,
    Inky,
    Clyde
}
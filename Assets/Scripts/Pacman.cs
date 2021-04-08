using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Pacman : MonoBehaviour
{
    private PacmanOrientation _pacmanOrientation;
    private PacmanAnimation _animation;
    private PacmanConsume _pacmanConsume;
    private PacmanMove _pacmanMovement;
    
    public Node startingPosition;
    public Vector2 direction = Vector2.zero;

    private void Start()
    {
        direction = Vector2.right;
        _pacmanOrientation = GetComponent<PacmanOrientation>();
        _animation = GetComponent<PacmanAnimation>();
        _pacmanConsume = GetComponent<PacmanConsume>();
        _pacmanMovement = GetComponent<PacmanMove>();
    }

    private void Update()
    {
        _pacmanMovement.MoveSprite();
        _pacmanOrientation.UpdateOrientation(direction);
        _animation.UpdateAnimation(direction);
        _pacmanConsume.ConsumePellet();
        _pacmanMovement.CheckInput();
    }

    public void Restart()
    {
        transform.position = startingPosition.transform.position;
        _pacmanMovement.currentNode = startingPosition;
        direction = Vector2.right;
        _pacmanOrientation.UpdateOrientation(direction);
        _pacmanMovement.nextDirection = direction;
        _pacmanMovement.ChangePacmanPosition(direction);
    }
}

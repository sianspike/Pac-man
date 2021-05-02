using System;
using UnityEngine;

namespace Pacman
{
    public class Pacman : MonoBehaviour
    {
        public static float pacmanSpeed;
        
        private PacmanOrientation _pacmanOrientation;
        private PacmanAnimation _animation;
        private PacmanConsume _pacmanConsume;
        private PacmanMove _pacmanMovement;
        
        public bool canMove = true;

        private void Start()
        {
            _pacmanMovement = GetComponent<PacmanMove>();
            _pacmanMovement.direction = Vector2.right;
            _pacmanOrientation = GetComponent<PacmanOrientation>();
            _animation = GetComponent<PacmanAnimation>();
            _pacmanConsume = GetComponent<PacmanConsume>();
        }

        private void Update()
        {
            if (canMove)
            {
                _pacmanMovement.CheckInput();
                _pacmanMovement.MoveSprite();
                _pacmanOrientation.UpdateOrientation(_pacmanMovement.direction);
                _animation.UpdateAnimation(_pacmanMovement.direction);
                _pacmanConsume.ConsumePellet();
            }
        }
        
        public void Restart()
        {
            canMove = true;
        }
    }
}
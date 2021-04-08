using UnityEngine;

namespace Pacman
{
    public class Pacman : MonoBehaviour
    {
        private PacmanOrientation _pacmanOrientation;
        private PacmanAnimation _animation;
        private PacmanConsume _pacmanConsume;
        private PacmanMove _pacmanMovement;
    
        public Node startingPosition;
        public Vector2 direction = Vector2.zero;
        public bool canMove = true;

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
            if (canMove)
            {
                _pacmanMovement.CheckInput();
                _pacmanMovement.MoveSprite();
                _pacmanOrientation.UpdateOrientation(direction);
                _animation.UpdateAnimation(direction);
                _pacmanConsume.ConsumePellet();
            }
        }

        public void Restart()
        {
            canMove = true;
            _animation.animator.runtimeAnimatorController = _animation.chompAnimation;
            _animation.animator.enabled = true;
            _animation.spriteRenderer.enabled = true;
            transform.position = startingPosition.transform.position;
            _pacmanMovement.currentNode = startingPosition;
            direction = Vector2.right;
            _pacmanOrientation.UpdateOrientation(direction);
            _pacmanMovement.nextDirection = direction;
            _pacmanMovement.ChangePacmanPosition(direction);
        }
    }
}
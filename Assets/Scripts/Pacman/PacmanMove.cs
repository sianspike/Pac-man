using UnityEngine;

namespace Pacman
{
    public class PacmanMove: Move
    {
        private PacmanAnimation _animation;
        private PacmanOrientation _orientation;
        
        private new void Start()
        { 
            base.Start();
            
            _animation = transform.GetComponent<PacmanAnimation>();
            _orientation = transform.GetComponent<PacmanOrientation>();
            direction = Vector2.right;
            targetNode = PacmanCanMove(direction);
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
        
        public void MoveSprite()
        {
            if (targetNode != currentNode || !ReferenceEquals(targetNode, null))
            {
                //if pacman wants to move in opposite inDirection before node is reached
                if (nextDirection == direction * -1)
                {
                    direction *= -1;
                
                    var temporaryNode = targetNode;
                            
                    targetNode = previousNode; 
                    previousNode = temporaryNode;
                }
                        
                if (OvershotTarget())
                {
                    currentNode = targetNode;
                    transform.localPosition = currentNode.transform.position;
                            
                    var moveToNode = PacmanCanMove(nextDirection);
                            
                    if (!ReferenceEquals(moveToNode, null))
                    {
                        direction = nextDirection;
                    }
                            
                    if (ReferenceEquals(moveToNode, null))
                    {
                        moveToNode = PacmanCanMove(direction);
                    }
                            
                    if (!ReferenceEquals(moveToNode, null))
                    {
                        targetNode = moveToNode;
                        previousNode = currentNode;
                        currentNode = null;
                    }
                    else
                    {
                        direction = Vector2.zero;
                    }
                }
                else
                {
                    transform.localPosition += (Vector3) (direction * speed) * Time.deltaTime;
                }
            }
        }
        
        private Node PacmanCanMove(Vector2 inDirection)
        {
            Node moveToNode = null;

            //iterate over neighbours of pellet
            for (var i = 0; i < currentNode.neighbours.Length; i++)
            {
                //check if current inDirection of pacman is valid
                if (currentNode.validDirections[i] != inDirection) continue;
            
                moveToNode = currentNode.neighbours[i];
            
                break;
            }

            return moveToNode;
        }

        public void ChangePacmanPosition(Vector2 targetDirection)
        {
            if (targetDirection != direction)
            {
                nextDirection = targetDirection;
            }

            if (!ReferenceEquals(currentNode, null))
            {
                var moveToNode = PacmanCanMove(targetDirection);

                if (!ReferenceEquals(moveToNode, null))
                {
                    direction = targetDirection;
                    targetNode = moveToNode;
                    previousNode = currentNode;
                    currentNode = null;
                }
            }
        }

        public new void MoveToStartingPosition()
        {
            base.MoveToStartingPosition();
            
            direction = Vector2.right;
            currentNode = startingPosition;
            nextDirection = direction;
            
            _animation.animator.runtimeAnimatorController = _animation.chompAnimation;
            _animation.animator.enabled = true;
            
            _orientation.UpdateOrientation(direction);
            ChangePacmanPosition(direction);
        }
    }
}
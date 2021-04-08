using UnityEngine;

namespace Pacman
{
    public class PacmanMove: Move
    {
        private global::Pacman.Pacman _pacman;
        
        private new void Start()
        { 
            base.Start();
            _pacman = transform.GetComponent<global::Pacman.Pacman>();
            _pacman.direction = Vector2.right;
            targetNode = PacmanCanMove(_pacman.direction);
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
                //if pacman wants to move in opposite direction before node is reached
                if (nextDirection == _pacman.direction * -1)
                {
                    _pacman.direction *= -1;
                
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
                        _pacman.direction = nextDirection;
                    }
                            
                    if (ReferenceEquals(moveToNode, null))
                    {
                        moveToNode = PacmanCanMove(_pacman.direction);
                    }
                            
                    if (!ReferenceEquals(moveToNode, null))
                    {
                        targetNode = moveToNode;
                        previousNode = currentNode;
                        currentNode = null;
                    }
                    else
                    {
                        _pacman.direction = Vector2.zero;
                    }
                }
                else
                {
                    transform.localPosition += (Vector3) (_pacman.direction * speed) * Time.deltaTime;
                }
            }
        }
        
        private Node PacmanCanMove(Vector2 direction)
        {
            Node moveToNode = null;

            //iterate over neighbours of pellet
            for (var i = 0; i < currentNode.neighbours.Length; i++)
            {
                //check if current direction of pacman is valid
                if (currentNode.validDirections[i] != direction) continue;
            
                moveToNode = currentNode.neighbours[i];
            
                break;
            }

            return moveToNode;
        }

        public void ChangePacmanPosition(Vector2 targetDirection)
        {
            if (targetDirection != _pacman.direction)
            {
                nextDirection = targetDirection;
            }

            if (!ReferenceEquals(currentNode, null))
            {
                var moveToNode = PacmanCanMove(targetDirection);

                if (!ReferenceEquals(moveToNode, null))
                {
                    _pacman.direction = targetDirection;
                    targetNode = moveToNode;
                    previousNode = currentNode;
                    currentNode = null;
                }
            }
        }
    }
}
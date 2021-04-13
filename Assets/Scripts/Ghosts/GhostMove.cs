using ScriptResources;
using UnityEngine;

namespace Ghosts
{
    public class GhostMove: Move
    {
        private Ghost _ghost;
        private Pacman.Pacman _pacman;
        private GhostMode _ghostMode;
        private GhostAnimation _animation;

        private new void Start()
        { 
            base.Start();

            _ghost = transform.GetComponent<Ghost>();
            _pacman = FindObjectOfType<Pacman.Pacman>();
            _ghostMode = transform.GetComponent<GhostMode>();
            _animation = transform.GetComponent<GhostAnimation>();
            currentNode = startingPosition;
            previousNode = startingPosition;

            if (_ghost.isInGhostHouse)
            {
                direction = Vector2.up;
                targetNode = currentNode.neighbours[0];
                
            } else
            {
                direction = Vector2.right;
                targetNode = ChooseNextNode();
            }
        }

        public Vector2 MoveSprite()
        {
            if (targetNode != currentNode && !ReferenceEquals(targetNode, null) && !_ghost.isInGhostHouse)
            {
                if (OvershotTarget())
                {
                    currentNode = targetNode;
                    transform.localPosition = currentNode.transform.position;
                    targetNode = ChooseNextNode();
                    previousNode = currentNode;
                    currentNode = null;
                }
                else
                {
                    transform.localPosition += (Vector3) direction * (speed * Time.deltaTime); 
                }
            }

            return direction;
        }

        public Node ChooseNextNode()
        {
            Node moveToNode = null;
            var targetTile = Vector2.zero;

            if (_ghostMode.currentMode == Mode.Chase)
            {
                targetTile = GetTargetTile();
            
            } else if (_ghostMode.currentMode == Mode.Scatter)
            {
                targetTile = _ghost.homeNode.transform.position;
            
            } else if (_ghostMode.currentMode == Mode.Frightened)
            {
                targetTile = GetRandomTile();
            
            } else if (_ghostMode.currentMode == Mode.Consumed)
            {
                targetTile = _ghost.ghostHouse.transform.position;
            }

            var foundNeighbours = new Node[4];
            var foundNeighboursDirection = new Vector2[4];
            var nodeCounter = 0;

            for (var i = 0; i < currentNode.neighbours.Length; i++)
            {
                if (currentNode.validDirections[i] != direction * -1)
                {
                    if (_ghostMode.currentMode != Mode.Consumed)
                    {
                        var tile = GetTileAtPosition(currentNode.transform.position).transform.GetComponent<Tile>();

                        if (tile.isGhostHouseEntrance)
                        {
                            if (currentNode.validDirections[i] != Vector2.down)
                            {
                                foundNeighbours[nodeCounter] = currentNode.neighbours[i];
                                foundNeighboursDirection[nodeCounter] = currentNode.validDirections[i];
                                nodeCounter++;
                            }
                        }
                        else
                        {
                            foundNeighbours[nodeCounter] = currentNode.neighbours[i];
                            foundNeighboursDirection[nodeCounter] = currentNode.validDirections[i];
                            nodeCounter++;
                        }
                    }
                    else
                    {
                        foundNeighbours[nodeCounter] = currentNode.neighbours[i];
                        foundNeighboursDirection[nodeCounter] = currentNode.validDirections[i];
                        nodeCounter++;
                    }
                }
            }

            if (foundNeighbours.Length == 1)
            {
                moveToNode = foundNeighbours[0];
                direction = foundNeighboursDirection[0];
            }

            if (foundNeighbours.Length > 1)
            {
                var leastDistance = 10000f;
            
                for (var i = 0; i < foundNeighbours.Length; i++)
                {
                    if (foundNeighboursDirection[i] == Vector2.zero) continue;
                
                    var distance = GetDistance(foundNeighbours[i].transform.localPosition, 
                        targetTile);

                    if (!(distance < leastDistance)) continue;
                    
                    leastDistance = distance;
                    moveToNode = foundNeighbours[i];
                    direction = foundNeighboursDirection[i];
                }
            }
        
            return moveToNode;
        }
    
        private float GetDistance(Vector2 positionA, Vector2 positionB)
        {
            var xDistance = positionA.x - positionB.x;
            var yDistance = positionA.y - positionB.y;
            var distance = Mathf.Sqrt(xDistance * xDistance + yDistance * yDistance);

            return distance;
        }
    
        private Vector2 GetBlinkyTargetTile()
        {
            Vector2 pacmanPosition = _pacman.transform.localPosition;
            var targetTile = new Vector2(Mathf.RoundToInt(pacmanPosition.x), Mathf.RoundToInt(pacmanPosition.y));

            return targetTile;
        }

        private Vector2 GetPinkyTargetTile()
        {
            //4 tiles ahead of pacman
            Vector2 pacmanPosition = _pacman.transform.localPosition;
            var pacmanPositionX = Mathf.RoundToInt(pacmanPosition.x);
            var pacmanPositionY = Mathf.RoundToInt(pacmanPosition.y);
            var pacmanTile = new Vector2(pacmanPositionX, pacmanPositionY);
            var targetTile = pacmanTile + (4 * pacmanPosition);

            return targetTile;
        }

        private Vector2 GetInkyTargetTile()
        {
            //2 tiles ahead of pacman
            Vector2 pacmanPosition = _pacman.transform.localPosition;
            Vector2 blinkyPosition = GameObject.Find("blinky").transform.localPosition;
            var pacmanPositionX = Mathf.RoundToInt(pacmanPosition.x);
            var pacmanPositionY = Mathf.RoundToInt(pacmanPosition.y);
            var pacmanTile = new Vector2(pacmanPositionX, pacmanPositionY);
            var targetTile = pacmanTile + (2 * pacmanPosition);
            var blinkyPositionX = Mathf.RoundToInt(blinkyPosition.x);
            var blinkyPositionY = Mathf.RoundToInt(blinkyPosition.y);

            blinkyPosition = new Vector2(blinkyPositionX, blinkyPositionY);

            var distance = GetDistance(blinkyPosition, targetTile);

            distance *= 2;
            targetTile = new Vector2(blinkyPosition.x + distance, blinkyPosition.y + distance);

            return targetTile;
        }

        private Vector2 GetClydeTargetTile()
        {
            //same as Blinky unless less than 8 tiles away from pacman
            Vector2 pacmanPosition = _pacman.transform.localPosition;
            Vector2 clydePosition = _ghost.transform.localPosition;
            var distance = GetDistance(clydePosition, pacmanPosition);
            var targetTile = Vector2.zero;

            if (distance > 8)
            {
                targetTile = new Vector2(Mathf.RoundToInt(pacmanPosition.x), Mathf.RoundToInt(pacmanPosition.y));
            
            } else if (distance < 8)
            {
                targetTile = _ghost.homeNode.transform.position;
            }

            return targetTile;
        }
    
        private Vector2 GetTargetTile()
        {
            var targetTile = Vector2.zero;

            if (_ghost.ghostType == GhostType.Blinky)
            {
                targetTile = GetBlinkyTargetTile();
            
            } else if (_ghost.ghostType == GhostType.Pinky)
            {
                targetTile = GetPinkyTargetTile();
            
            } else if (_ghost.ghostType == GhostType.Inky)
            {
                targetTile = GetInkyTargetTile();
            
            } else if (_ghost.ghostType == GhostType.Clyde)
            {
                targetTile = GetClydeTargetTile();
            }

            return targetTile;
        }

        private void ReleasePinky()
        {
            if (_ghost.ghostType == GhostType.Pinky && _ghost.isInGhostHouse)
            {
                _ghost.isInGhostHouse = false;
                targetNode = currentNode.neighbours[0];
            }
        }

        private void ReleaseInky()
        {
            if (_ghost.ghostType == GhostType.Inky && _ghost.isInGhostHouse)
            {
                _ghost.isInGhostHouse = false;
            }
        }

        private void ReleaseClyde()
        {
            if (_ghost.ghostType == GhostType.Clyde && _ghost.isInGhostHouse)
            {
                _ghost.isInGhostHouse = false;
            }
        }

        public void ReleaseGhosts()
        {
            _ghost.ghostReleaseTimer += Time.deltaTime;

            if (_ghost.ghostReleaseTimer > _ghost.pinkyReleaseTimer)
            {
                ReleasePinky();
            }
        
            if (_ghost.ghostReleaseTimer > _ghost.inkyReleaseTimer)
            {
                ReleaseInky();
            }
        
            if (_ghost.ghostReleaseTimer > _ghost.clydeReleaseTimer)
            {
                ReleaseClyde();
            }
        }

        private Vector2 GetRandomTile()
        {
            var x = Random.Range(0, 31);
            var y = Random.Range(0, 31);

            return new Vector2(x, y);
        }
        
        public new void MoveToStartingPosition()
        {
            base.MoveToStartingPosition();
            
            currentNode = startingPosition;
            previousNode = currentNode;

            _animation.UpdateAnimation(direction);
            
            if (transform.name != "blinky")
            {
                _ghost.isInGhostHouse = true;
            }
        }
    }
}
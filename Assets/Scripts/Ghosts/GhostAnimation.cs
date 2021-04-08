using ScriptResources;
using UnityEngine;

namespace Ghosts
{
    public class GhostAnimation: SpriteAnimation
    {
        private Ghost _ghost;
        private GhostMode _ghostMode;
        private new void Start()
        {
            base.Start();

            _ghost = transform.GetComponent<Ghost>();
            _ghostMode = transform.GetComponent<GhostMode>();
            animator.runtimeAnimatorController = _ghost.ghostRight;
        }
        
        public void UpdateAnimation(Vector2 direction)
        {
            if (_ghostMode.currentMode != Mode.Frightened && _ghostMode.currentMode != Mode.Consumed)
            {
                if (direction == Vector2.left)
                {
                    animator.runtimeAnimatorController = _ghost.ghostLeft;
                
                } else if (direction == Vector2.right)
                {
                    animator.runtimeAnimatorController = _ghost.ghostRight;
                
                } else if (direction == Vector2.up)
                {
                    animator.runtimeAnimatorController = _ghost.ghostUp;
                
                } else if (direction == Vector2.down)
                {
                    animator.runtimeAnimatorController = _ghost.ghostDown;
                }
            
            } else if (_ghostMode.currentMode == Mode.Frightened)
            {
                animator.runtimeAnimatorController = _ghost.ghostFrightened;
            
                if (_ghostMode.frightenedModeTimer >= _ghost.startBlinkingAt)
                {
                    _ghost.blinkTimer += Time.deltaTime;

                    if (_ghost.blinkTimer >= 0.1f)
                    {
                        _ghost.blinkTimer = 0f;

                        if (_ghost.ghostIsWhite)
                        {
                            animator.runtimeAnimatorController = _ghost.ghostFrightened;
                            _ghost.ghostIsWhite = false;
                        }
                        else
                        {
                            animator.runtimeAnimatorController = _ghost.ghostWhite;
                            _ghost.ghostIsWhite = true;
                        }
                    }
                }
            
            } else if (_ghostMode.currentMode == Mode.Consumed)
            {
                if (direction == Vector2.left)
                {
                    animator.runtimeAnimatorController = null;
                    spriteRenderer.sprite = _ghost.eyesLeft;
                }
                else if (direction == Vector2.right)
                {

                    animator.runtimeAnimatorController = null;
                    spriteRenderer.sprite = _ghost.eyesRight;
            
                }
                else if (direction == Vector2.up)
                {

                    animator.runtimeAnimatorController = null;
                    spriteRenderer.sprite = _ghost.eyesUp;
            
                }
                else if (direction == Vector2.down)
                {
                    animator.runtimeAnimatorController = null;
                    spriteRenderer.sprite = _ghost.eyesDown;
                }
            }
        }
    }
}
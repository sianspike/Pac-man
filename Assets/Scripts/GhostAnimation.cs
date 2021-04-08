using ScriptResources;
using UnityEngine;

public class GhostAnimation: SpriteAnimation
{
    private Ghost _ghost;
    private GhostMode _ghostMode;
    private new void Start()
    {
        base.Start();

        _ghost = transform.GetComponent<Ghost>();
        _ghostMode = transform.GetComponent<GhostMode>();
        Animator.runtimeAnimatorController = _ghost.ghostRight;
    }
        
    public void UpdateAnimation(Vector2 direction)
    {
        if (_ghostMode.currentMode != Mode.Frightened && _ghostMode.currentMode != Mode.Consumed)
        {
            if (direction == Vector2.left)
            {
                Animator.runtimeAnimatorController = _ghost.ghostLeft;
                
            } else if (direction == Vector2.right)
            {
                Animator.runtimeAnimatorController = _ghost.ghostRight;
                
            } else if (direction == Vector2.up)
            {
                Animator.runtimeAnimatorController = _ghost.ghostUp;
                
            } else if (direction == Vector2.down)
            {
                Animator.runtimeAnimatorController = _ghost.ghostDown;
            }
            
        } else if (_ghostMode.currentMode == Mode.Frightened)
        {
            Animator.runtimeAnimatorController = _ghost.ghostFrightened;
            
            if (_ghostMode.frightenedModeTimer >= _ghost.startBlinkingAt)
            {
                _ghost.blinkTimer += Time.deltaTime;

                if (_ghost.blinkTimer >= 0.1f)
                {
                    _ghost.blinkTimer = 0f;

                    if (_ghost.ghostIsWhite)
                    {
                        Animator.runtimeAnimatorController = _ghost.ghostFrightened;
                        _ghost.ghostIsWhite = false;
                    }
                    else
                    {
                        Animator.runtimeAnimatorController = _ghost.ghostWhite;
                        _ghost.ghostIsWhite = true;
                    }
                }
            }
            
        } else if (_ghostMode.currentMode == Mode.Consumed)
        {
            if (direction == Vector2.left)
            {
                Animator.runtimeAnimatorController = null;
                SpriteRenderer.sprite = _ghost.eyesLeft;
            }
            else if (direction == Vector2.right)
            {

                Animator.runtimeAnimatorController = null;
                SpriteRenderer.sprite = _ghost.eyesRight;
            
            }
            else if (direction == Vector2.up)
            {

                Animator.runtimeAnimatorController = null;
                SpriteRenderer.sprite = _ghost.eyesUp;
            
            }
            else if (direction == Vector2.down)
            {
                Animator.runtimeAnimatorController = null;
                SpriteRenderer.sprite = _ghost.eyesDown;
            }
        }
    }
}
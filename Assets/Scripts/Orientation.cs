using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Orientation : MonoBehaviour
{
    private Animator _animator;
    private Ghost _ghost;

    private void Start()
    {
        _animator = transform.GetComponent<Animator>();

        if (gameObject.CompareTag("ghost"))
        {
            _ghost = gameObject.GetComponent<Ghost>();
            _animator.runtimeAnimatorController = _ghost.ghostLeft;
        }
    }
    
    public void UpdateOrientation(Vector2 direction)
    {
        if (direction == Vector2.left)
        {
            if (!gameObject.CompareTag("ghost"))
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                
            } else if (gameObject.CompareTag("ghost") && _ghost.currentMode != Mode.Frightened)
            {
                _animator.runtimeAnimatorController = _ghost.ghostLeft;
                
            } else if (gameObject.CompareTag("ghost") && _ghost.currentMode == Mode.Frightened)
            {
                _animator.runtimeAnimatorController = _ghost.ghostFrightened;
            }
            
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (direction == Vector2.right)
        {
            if (!gameObject.CompareTag("ghost"))
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                
            } else if (gameObject.CompareTag("ghost") && _ghost.currentMode != Mode.Frightened)
            {
                _animator.runtimeAnimatorController = _ghost.ghostRight;
                
            } else if (gameObject.CompareTag("ghost") && _ghost.currentMode == Mode.Frightened)
            {
                _animator.runtimeAnimatorController = _ghost.ghostFrightened;
            }
            
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (direction == Vector2.up)
        {
            if (!gameObject.CompareTag("ghost"))
            {
                transform.localRotation = Quaternion.Euler(0, 0, 90);
                
            } else if (gameObject.CompareTag("ghost") && _ghost.currentMode != Mode.Frightened)
            {
                _animator.runtimeAnimatorController = _ghost.ghostUp;
                
            } else if (gameObject.CompareTag("ghost") && _ghost.currentMode == Mode.Frightened)
            {
                _animator.runtimeAnimatorController = _ghost.ghostFrightened;
            }
            
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (direction == Vector2.down)
        {
            if (!gameObject.CompareTag("ghost"))
            {
                transform.localRotation = Quaternion.Euler(0, 0, 270);
                
            } else if (gameObject.CompareTag("ghost") && _ghost.currentMode != Mode.Frightened)
            {
                _animator.runtimeAnimatorController = _ghost.ghostDown;
                
            } else if (gameObject.CompareTag("ghost") && _ghost.currentMode == Mode.Frightened)
            {
                if (_ghost.frightenedModeTimer >= _ghost.startBlinkingAt)
                {
                    _ghost.blinkTimer += Time.deltaTime;

                    if (_ghost.blinkTimer >= 0.1f)
                    {
                        _ghost.blinkTimer = 0f;

                        if (_ghost.ghostIsWhite)
                        {
                            _animator.runtimeAnimatorController = _ghost.ghostFrightened;
                            _ghost.ghostIsWhite = false;
                        }
                        else
                        {
                            _animator.runtimeAnimatorController = _ghost.ghostWhite;
                            _ghost.ghostIsWhite = true;
                        }
                    }
                }
            }
            
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}

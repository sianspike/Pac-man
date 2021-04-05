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
            }
            else
            {
                _animator.runtimeAnimatorController = _ghost.ghostLeft;
            }
            
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (direction == Vector2.right)
        {
            if (!gameObject.CompareTag("ghost"))
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                _animator.runtimeAnimatorController = _ghost.ghostRight;
            }
            
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (direction == Vector2.up)
        {
            if (!gameObject.CompareTag("ghost"))
            {
                transform.localRotation = Quaternion.Euler(0, 0, 90);
            }
            else
            {
                _animator.runtimeAnimatorController = _ghost.ghostUp;
            }
            
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (direction == Vector2.down)
        {
            if (!gameObject.CompareTag("ghost"))
            {
                transform.localRotation = Quaternion.Euler(0, 0, 270);
            }
            else
            {
                _animator.runtimeAnimatorController = _ghost.ghostDown;
            }
            
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}

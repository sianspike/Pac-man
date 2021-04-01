using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacmanAnimation : MonoBehaviour
{
    [SerializeField] public Sprite nonMovingPacman;
    
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    public void UpdateAnimation(Vector2 direction)
    {
        //pacman not moving
        if (direction == Vector2.zero)
        {
            _animator.enabled = false;
            _spriteRenderer.sprite = nonMovingPacman;
        }
        else
        {
            _animator.enabled = true;
        }
    }
}

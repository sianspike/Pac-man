using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacmanAnimation : SpriteAnimation
{
    [SerializeField] public Sprite nonMovingPacman;

    private new void Start()
    {
        base.Start();
    }
    
    public void UpdateAnimation(Vector2 direction)
    {
        //pacman not moving
        if (direction == Vector2.zero)
        {
            Animator.enabled = false;
            SpriteRenderer.sprite = nonMovingPacman;
        }
        else
        {
            Animator.enabled = true;
        }
    }
}

using System;
using ScriptResources;
using UnityEngine;

public class Collision : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private GameObject _pacman;
    private SpriteRenderer _pacmanSpriteRenderer;
    private Consume _consume;

    private void Start()
    {
        _spriteRenderer = transform.GetComponent<SpriteRenderer>();
        _pacman = GameObject.FindGameObjectWithTag("pacman");
        _pacmanSpriteRenderer = _pacman.GetComponent<SpriteRenderer>();
        _consume = GetComponent<Consume>();
    }

    public void CheckCollision(GhostMode mode)
    {
        var ghostRect = new Rect(transform.position, _spriteRenderer.sprite.bounds.size / 4);
        var pacmanRect = new Rect(_pacman.transform.position, _pacmanSpriteRenderer.sprite.bounds.size / 4);

        if (ghostRect.Overlaps(pacmanRect))
        {
            if (mode.currentMode == Mode.Frightened)
            {
                _consume.Consumed();
            }
        }
    }    
}
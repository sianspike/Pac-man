using UnityEngine;

public class SpriteAnimation: MonoBehaviour
{
        protected SpriteRenderer SpriteRenderer;
        protected Animator Animator;
        
        protected void Start()
        { 
                Animator = GetComponent<Animator>();
                SpriteRenderer = GetComponent<SpriteRenderer>();
        }
}
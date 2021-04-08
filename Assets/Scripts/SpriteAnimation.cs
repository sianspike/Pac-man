using UnityEngine;

public class SpriteAnimation: MonoBehaviour
{
        public SpriteRenderer spriteRenderer;
        public Animator animator;
        
        protected void Start()
        { 
                animator = GetComponent<Animator>();
                spriteRenderer = GetComponent<SpriteRenderer>();
        }
}
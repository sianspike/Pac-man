using System.Collections;
using UnityEngine;

namespace Pacman
{
    public class PacmanAnimation : SpriteAnimation
    {
        [SerializeField] public Sprite nonMovingPacman;

        public RuntimeAnimatorController chompAnimation;
        public RuntimeAnimatorController deathAnimation;

        private Pacman _pacman;
        private AudioSource _audioSource;
        private Audio _audio;

        private new void Start()
        {
            base.Start();

            _pacman = GetComponent<Pacman>();
            _audioSource = GetComponent<AudioSource>();
            _audio = FindObjectOfType<Audio>();
        }
    
        public void UpdateAnimation(Vector2 direction)
        {
            //pacman not moving
            if (direction == Vector2.zero)
            {
                animator.enabled = false;
                spriteRenderer.sprite = nonMovingPacman;
            }
            else
            {
                animator.enabled = true;
            }
        }
        
        public IEnumerator ProcessDeathAnimation(float delay)
        {
            _pacman.transform.localScale = new Vector3(1, 1, 1);
            _pacman.transform.localRotation = Quaternion.Euler(0, 0, 0);

            animator.runtimeAnimatorController = deathAnimation;
            animator.enabled = true;
            _audioSource.clip = _audio.pacmanDeathBackgroundAudio;
        
            _audioSource.Play();

            yield return new WaitForSeconds(delay);

            StartCoroutine(GameBoard.instance.ProcessRestart(1));
        }
    }
}

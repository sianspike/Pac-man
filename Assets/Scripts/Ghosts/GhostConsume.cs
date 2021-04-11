using System.Collections;
using ScriptResources;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace Ghosts
{
    public class GhostConsume: MonoBehaviour
    {
        private GhostMode _mode;
        private GhostMove _ghostMovement;
        private bool _didStartConsumed;
        private Ghost[] _ghosts;
        private Pacman.Pacman _pacman;
        private SpriteRenderer _pacmanSpriteRenderer;
        private AudioSource _audioSource;
        private RectTransform _consumeScoreRectTransform;
        private Audio _audio;
        private Ghost _ghost;

        private void Start()
        {
            _mode = GetComponent<GhostMode>();
            _ghostMovement = GetComponent<GhostMove>();
            _ghosts = FindObjectsOfType<Ghost>();
            _pacman = FindObjectOfType<Pacman.Pacman>();
            _pacmanSpriteRenderer = _pacman.GetComponent<SpriteRenderer>();
            _audioSource = FindObjectOfType<AudioSource>();
            _consumeScoreRectTransform = GameBoard.instance.consumedGhostScoreText.GetComponent<RectTransform>();
            _audio = FindObjectOfType<Audio>();
            _ghost = transform.GetComponent<Ghost>();
        }
    
        public void Consumed()
        {
            GameBoard.instance.score += 200;
            _mode.currentMode = Mode.Consumed;
            _mode.previousSpeed = _ghostMovement.speed;
            _ghostMovement.speed = _mode.consumedSpeed;
            
            StartConsumed(_ghost);
        }
        
        private IEnumerator ProcessConsumedAfter(float delay, Ghost consumedGhost)
        {
            yield return new WaitForSeconds(delay);

            GameBoard.instance.consumedGhostScoreText.enabled = false;
            _pacmanSpriteRenderer.enabled = true;
            consumedGhost.GetComponent<SpriteRenderer>().enabled = true;
            
            foreach (var ghost in _ghosts)
            {
                ghost.canMove = true;
            }
            
            _pacman.canMove = true;
            
            _audioSource.Play();

            _didStartConsumed = false;
        }

        private void StartConsumed(Ghost consumedGhost)
        {
            if (!_didStartConsumed)
            {
                _didStartConsumed = true;

                foreach (var ghost in _ghosts)
                {
                    ghost.canMove = false;
                }

                _pacman.canMove = false;
                _pacmanSpriteRenderer.enabled = false;
                consumedGhost.GetComponent<SpriteRenderer>().enabled = false;
                _audioSource.Stop();

                Vector2 consumedGhostPosition = consumedGhost.transform.position;
                Vector2 viewPortPoint = Camera.main.WorldToViewportPoint(consumedGhostPosition);
                
                _consumeScoreRectTransform.anchorMin = viewPortPoint;
                _consumeScoreRectTransform.anchorMax = viewPortPoint;
                GameBoard.instance.consumedGhostScoreText.enabled = true;
                _audioSource.PlayOneShot(_audio.consumedGhostAudio);

                StartCoroutine(ProcessConsumedAfter(0.75f, consumedGhost));
            }
        }
    }
}

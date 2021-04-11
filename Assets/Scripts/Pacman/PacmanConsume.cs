using UnityEngine;

namespace Pacman
{
    public class PacmanConsume: MonoBehaviour
    {
        private int _pelletsConsumed;
        private Audio _audio;
        private GameObject[] _ghostGameObjects;
        private PacmanMove _pacmanMove;
        private GameObject _game;

        private void Start()
        {
            _game = GameObject.Find("game");
            _audio = _game.transform.GetComponent<Audio>();
            _ghostGameObjects = GameObject.FindGameObjectsWithTag("ghost");
            _pacmanMove = GetComponent<PacmanMove>();
        }
    
        public void ConsumePellet()
        {
            var tileObject = _pacmanMove.GetTileAtPosition(transform.position);

            if (ReferenceEquals(tileObject, null)) return;
        
            var tile = tileObject.GetComponent<Tile>();

            if (ReferenceEquals(tile, null)) return;

            if (!tile.consumed && (tile.isPellet || tile.isSuperPellet))
            {
                tileObject.GetComponent<SpriteRenderer>().enabled = false;
                tile.consumed = true;
                GameBoard.instance.score += 10;
                _pelletsConsumed++;
                
                _audio.PlayChompSound();

                if (tile.isSuperPellet)
                {
                    foreach (var ghost in _ghostGameObjects)
                    {
                        ghost.GetComponent<GhostMode>().StartFrightenedMode();
                    }
                }
            }
        }
    }
}
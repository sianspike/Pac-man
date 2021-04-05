using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Consume : MonoBehaviour
{
    private int _pelletsConsumed;
    private Audio _audio;

    private void Start()
    {
        _audio = GetComponent<Audio>();
    }
    
    private GameObject GetTileAtPosition(Vector2 position, GameBoard gameBoard)
    {
        var tileX = Mathf.RoundToInt(position.x);
        var tileY = Mathf.RoundToInt(position.y);
        var tile = gameBoard.Board[tileX, tileY];

        return tile;
    }

    public void ConsumePellet(GameBoard gameBoard)
    {
        var tileObject = GetTileAtPosition(transform.position, gameBoard);

        if (ReferenceEquals(tileObject, null)) return;
        
        var tile = tileObject.GetComponent<Tile>();

        if (ReferenceEquals(tile, null)) return;

        if (!tile.consumed && (tile.isPellet || tile.isSuperPellet))
        {
            tileObject.GetComponent<SpriteRenderer>().enabled = false;
            tile.consumed = true;
            gameBoard.Score++;
            _pelletsConsumed++;
            _audio.PlayChompSound();
        }
    }
}

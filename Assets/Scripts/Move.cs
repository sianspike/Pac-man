using UnityEngine;

public class Move : MonoBehaviour
{
    public Node currentNode, targetNode, previousNode;
    public Vector2 nextDirection;
    public Node startingPosition;
    public Vector2 direction = Vector2.zero;

    protected void Start()
    {
        var node = GetNodeAtPosition(transform.position);

        if (node != null)
        {
            currentNode = node;
            previousNode = currentNode;
        }
    }

    public Node GetNodeAtPosition(Vector2 position)
    {
        var tile = GameBoard.instance.board[(int) position.x, (int) position.y];

        return tile != null ? tile.GetComponent<Node>() : null;
    }

    private float LengthFromNode(Vector2 targetPosition)
    {
        var length = targetPosition - (Vector2) previousNode.transform.position;

        return length.sqrMagnitude;
    }

    protected bool OvershotTarget()
    {
        var nodeToTarget = LengthFromNode(targetNode.transform.position);
        var nodeToSelf = LengthFromNode(transform.localPosition);

        return nodeToSelf > nodeToTarget;
    }

    public GameObject GetTileAtPosition(Vector2 position)
    {
        var tileX = Mathf.RoundToInt(position.x);
        var tileY = Mathf.RoundToInt(position.y);
        var tile = GameBoard.instance.board[tileX, tileY];

        if (tile != null)
        {
            return tile;
        }

        return null;
    }

    protected void MoveToStartingPosition()
    {
        transform.position = startingPosition.transform.position;
    }

}
using UnityEngine;

public class Move : MonoBehaviour 
{
    [SerializeField] public float speed;

    public Node currentNode, targetNode, previousNode;
    public Vector2 nextDirection;

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
        var tile = GameBoard.Instance.Board[(int) position.x, (int) position.y];
        
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
        var tile = GameBoard.Instance.Board[tileX, tileY];

        if (tile != null)
        {
            return tile;
        }

        return null;
    }
}
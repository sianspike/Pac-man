using UnityEngine;


public class Node : MonoBehaviour
{
    [SerializeField] public Node[] neighbours;
    
    public Vector2[] validDirections;
    
    // Start is called before the first frame update
    private void Start()
    {
        validDirections = new Vector2[neighbours.Length];

        for (var i = 0; i < neighbours.Length; i++)
        {
            var neighbour = neighbours[i];
            Vector2 direction = neighbour.transform.localPosition - transform.localPosition;

            validDirections[i] = direction.normalized;
        }
    }
}

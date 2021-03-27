using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] public Node[] neighbours;
    private Vector2[] _validDirections;
    
    // Start is called before the first frame update
    void Start()
    {

        _validDirections = new Vector2[neighbours.Length];

        for (int i = 0; i < neighbours.Length; i++)
        {
            Node neighbour = neighbours[i];
            Vector2 direction = neighbour.transform.localPosition - transform.localPosition;

            _validDirections[i] = direction.normalized;
        }
    }
}

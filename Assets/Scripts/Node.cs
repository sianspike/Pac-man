using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{

    [SerializeField] public Node[] neighbours;
    [SerializeField] public Vector2[] validDirections;
    
    // Start is called before the first frame update
    void Start()
    {

    }
}

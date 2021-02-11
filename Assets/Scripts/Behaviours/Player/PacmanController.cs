using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PacmanController : MonoBehaviour
{
    [SerializeField] private float speed;
    
    private Rigidbody2D rigidBody;
    private Vector2 inputDirection;
    private Vector2 velocity;

    private void Awake()
    {
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move(inputDirection);
    }

    public void Move(Vector2 direction)
    {
        velocity = rigidBody.velocity;
        velocity.x = direction.x * speed;
        rigidBody.velocity = velocity;
    }

    public void OnMove(InputValue input)
    {
        inputDirection = input.Get<Vector2>();
    }
}

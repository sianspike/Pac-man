using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PacmanController : MonoBehaviour
{
    [SerializeField] private float speed;
    
    private Rigidbody2D _rigidBody;
    private Vector2 _inputDirection;
    private Vector2 _velocity;

    private void Awake()
    {
        _rigidBody = gameObject.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {

        _rigidBody.isKinematic = true;
        
        if (_inputDirection != Vector2.zero)
        {
            _rigidBody.isKinematic = false;
            Move(_inputDirection);
        }
    }

    private void Move(Vector2 direction)
    {
        _velocity = _rigidBody.velocity;
        _velocity.x = direction.x * speed * Time.deltaTime;
        _rigidBody.velocity = _velocity;
        // var xForce = direction.x * speed * Time.deltaTime;
        // var force = new Vector2(xForce, 0);
        // _rigidBody.AddForce(force);
    }

    public void OnMove(InputAction.CallbackContext input)
    {
        if (!input.started) return;
        _inputDirection = input.ReadValue<Vector2>();
        Debug.Log("Button pressed");
    }
}

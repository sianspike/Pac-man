using UnityEngine;

namespace Behaviours.Player
{
    public class PacmanMove : MonoBehaviour 
    {
        [SerializeField] public float speed = 4.0f;
        private Vector2 _direction = Vector2.zero;
        
        void Update()
        {
            CheckInput();
            Move();
            UpdateOrientation();
        }

        void CheckInput()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                _direction = Vector2.left;
            } 
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                _direction = Vector2.right;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                _direction = Vector2.up;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                _direction = Vector2.down;
            }
        }

        void Move()
        {
            transform.localPosition += (Vector3) (_direction * speed) * Time.deltaTime;
        }

        void UpdateOrientation()
        {
            if (_direction == Vector2.left)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else if (_direction == Vector2.right)
            {
                transform.localScale = new Vector3(1, 1, 1);
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else if (_direction == Vector2.up)
            {
                transform.localScale = new Vector3(1, 1, 1);
                transform.localRotation = Quaternion.Euler(0, 0, 90);
            }
            else if (_direction == Vector2.down)
            {
                transform.localScale = new Vector3(1, 1, 1);
                transform.localRotation = Quaternion.Euler(0, 0, 270);
            }
        }
    }
}
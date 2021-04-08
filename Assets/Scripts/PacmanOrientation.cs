using UnityEngine;

public class PacmanOrientation: MonoBehaviour
{
    public void UpdateOrientation(Vector2 direction)
    {
        if (direction == Vector2.left)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (direction == Vector2.right)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (direction == Vector2.up)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 90);
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (direction == Vector2.down)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 270);
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
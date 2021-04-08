using UnityEngine;

namespace Ghosts
{
    public class GhostOrientation: MonoBehaviour
    {
        public void UpdateOrientation(Vector2 direction)
        {
            if (direction == Vector2.left)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (direction == Vector2.right)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (direction == Vector2.up)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (direction == Vector2.down)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }
}
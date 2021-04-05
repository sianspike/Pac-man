using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Orientation : MonoBehaviour
{
    public void UpdateOrientation(Vector2 direction)
    {
        if (direction == Vector2.left)
        {
            if (!gameObject.CompareTag("ghost"))
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (direction == Vector2.right)
        {
            if (!gameObject.CompareTag("ghost"))
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (direction == Vector2.up)
        {
            if (!gameObject.CompareTag("ghost"))
            {
                transform.localRotation = Quaternion.Euler(0, 0, 90);
            }
            
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (direction == Vector2.down)
        {
            if (!gameObject.CompareTag("ghost"))
            {
                transform.localRotation = Quaternion.Euler(0, 0, 270);
            }
            
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}

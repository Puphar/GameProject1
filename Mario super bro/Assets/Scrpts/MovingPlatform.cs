using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pointA; // Starting position
    public Transform pointB; // Ending position
    public float speed = 2.0f; // Movement speed
    public bool moveAutomatically = true; // Whether the platform should move automatically

    private Vector3 currentTarget; // Current target position

    void Start()
    {
        currentTarget = pointB.position; // Start moving towards point B
    }

    void Update()
    {
        if (moveAutomatically)
        {
            MovePlatform();
        }
    }

    void MovePlatform()
    {
        // Move the platform towards the current target
        transform.position = Vector3.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);

        // If the platform reaches the current target, switch the target
        if (transform.position == currentTarget)
        {
            if (currentTarget == pointA.position)
            {
                currentTarget = pointB.position;
            }
            else
            {
                currentTarget = pointA.position;
            }
        }
    }

    // Optionally, you can add gizmos to visualize the path of the moving platform in the editor
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(pointA.position, 0.2f);
        Gizmos.DrawSphere(pointB.position, 0.2f);
        Gizmos.DrawLine(pointA.position, pointB.position);
    }
}

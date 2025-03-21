using UnityEngine;

public class PiranhaPlant : MonoBehaviour
{
    public float moveSpeed = 2f; // Adjust this to control the movement speed
    public Transform topPoint; // Top point of Piranha Plant movement
    public Transform bottomPoint; // Bottom point of Piranha Plant movement

    private bool movingUp = true; // Initially moving upwards
    private bool waitingAtTop = false; // Flag to indicate if Piranha Plant is waiting at the top
    private float waitTimer = 0f; // Timer to track how long Piranha Plant has been waiting at the top
    private float waitDuration = 1f; // Duration to wait at the top in seconds

    // Reference to the Enemy script attached to the Piranha Plant
    private Enemy enemy;

    // Reference to the Rigidbody2D component
    private Rigidbody2D rb;

    void Start()
    {
        // Add Rigidbody2D component and set it to kinematic
        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.isKinematic = true;

        // Ensure the Piranha Plant has an Enemy component
        enemy = GetComponent<Enemy>();
        if (enemy == null)
        {
            Debug.LogError("PiranhaPlant is missing Enemy component!");
        }
    }

    void Update()
    {
        // Move the Piranha Plant between top and bottom points
        if (movingUp && !waitingAtTop)
        {
            transform.position = Vector2.MoveTowards(transform.position, topPoint.position, moveSpeed * Time.deltaTime);
            if (transform.position == topPoint.position)
            {
                waitingAtTop = true; // Set flag to indicate Piranha Plant is waiting at the top
            }
        }
        else if (waitingAtTop)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitDuration)
            {
                movingUp = false; // Start moving down after waiting duration is reached
                waitingAtTop = false; // Reset the flag
                waitTimer = 0f; // Reset the timer
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, bottomPoint.position, moveSpeed * Time.deltaTime);
            if (transform.position == bottomPoint.position)
            {
                movingUp = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                // Check if the player has star power
                if (player.starpower)
                {
                    // Call the Hit method of the enemy if the player has star power
                    if (enemy != null)
                    {
                        enemy.Hit();
                    }
                }
                else
                {
                    // Call the Hit method of the player if the player doesn't have star power
                    player.Hit();
                }
            }
        }
    }

    // Method to draw gizmos for visualizing the top and bottom points
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(topPoint.transform.position, 0.5f);
        Gizmos.DrawWireSphere(bottomPoint.transform.position, 0.5f);
    }
}

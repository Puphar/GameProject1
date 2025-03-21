using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Move the fireball
        rb.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the fireball collides with an object that can take damage
        if (collision.CompareTag("Player"))
        {
            // Deal damage to the player
            /*Mario mario = collision.GetComponent<Mario>();
            if (mario != null)
            {
                mario.TakeDamage(damage);
            }*/

            // Destroy the fireball
            Destroy(gameObject);
        }
        else if (!collision.CompareTag("Enemy"))
        {
            // Destroy the fireball if it collides with anything other than the player or another enemy
            Destroy(gameObject);
        }
    }

    public void Launch(float direction)
    {
        // Set the velocity of the fireball based on the direction
        rb.velocity = new Vector2(speed * direction, rb.velocity.y);
    }
}

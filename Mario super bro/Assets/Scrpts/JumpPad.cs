using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float jumpBoostForce = 10f; // Adjust the jump boost force as needed

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player collided with JumpPad.");
            Rigidbody2D playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRigidbody != null)
            {
                // Apply an upward force to the player for jump boost
                Debug.Log("Applying jump boost to player.");
                playerRigidbody.AddForce(Vector2.up * jumpBoostForce, ForceMode2D.Impulse);
            }
            else
            {
                Debug.LogWarning("Player Rigidbody2D component not found.");
            }

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bowser : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform player;
    private float bowserSpeed = 2f;
    private float jumpForce = 5f;
    private bool isChasing = false;
    private float fireballSpeed = 3f;
    private float chaseSpeed = 3f;

    public GameObject fireballPrefab;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        StartCoroutine(BowserBehavior());
    }

    IEnumerator BowserBehavior()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1f, 3f));

            if (!isChasing)
            {
                Jump();
                yield return new WaitForSeconds(Random.Range(1f, 4f));
                MoveForward();
                yield return new WaitForSeconds(1f);
                ShootFireball();
            }
            else
            {
                ChasePlayer();
            }
        }
    }

    void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        //Debug.Log("Bowser jumps!");
    }

    void MoveForward()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * chaseSpeed;
        //Debug.Log("Bowser moves forward a little.");
    }

    void ShootFireball()
    {
        Debug.Log("Bowser shoots a fireball!");
        GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);

        // Calculate the direction towards the player
        Vector2 direction = (player.position - transform.position).normalized;

        // Get the Rigidbody2D component of the fireball
        Rigidbody2D rbFireball = fireball.GetComponent<Rigidbody2D>();

        // Apply force to move the fireball in the calculated direction
        rbFireball.AddForce(direction * fireballSpeed, ForceMode2D.Impulse);
    }

    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * chaseSpeed;
        Debug.Log("Bowser notices the player behind and continues chasing!");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isChasing = true;
        }
    }
}

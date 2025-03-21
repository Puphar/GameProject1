using UnityEngine;

public class Projectile : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.Hit();

            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Wall") )
        {
            Destroy(gameObject);
        }
    }
}

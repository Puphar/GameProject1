using UnityEngine;

public class FlyingKoopa : MonoBehaviour
{
    public Transform startPosition;
    public Transform endPosition;
    public float flyingSpeed = 5f;

    private bool shelled;
    private bool pushed;

    public GameObject normalKoopaPrefab;

    // You can keep other variables and methods from your normal Koopa script as needed

    private void Update()
    {
        // Implement flying movement logic for the Flying Koopa
        transform.position = Vector2.Lerp(startPosition.position, endPosition.position, Mathf.PingPong(Time.time * flyingSpeed, 1f));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Handle collision with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Transform Flying Koopa into Normal Koopa
            TransformToNormalKoopa();
        }
    }

    private void TransformToNormalKoopa()
    {
        // Disable the Flying Koopa
        gameObject.SetActive(false);

        // Instantiate a Normal Koopa at the same position
        GameObject normalKoopa = Instantiate(normalKoopaPrefab, transform.position, Quaternion.identity);
        Destroy(normalKoopa, 3f); // Destroy the normal Koopa after a delay if needed
    }
}

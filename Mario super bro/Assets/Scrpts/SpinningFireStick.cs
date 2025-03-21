using UnityEngine;

public class SpinningFireStick : MonoBehaviour
{
    public float rotationSpeed = 100f;
    public Transform pivotPoint;

    void Update()
    {
        if (pivotPoint != null)
        {
            transform.RotateAround(pivotPoint.position, Vector3.forward, rotationSpeed * Time.deltaTime);
        }
        else
        {
            Debug.LogWarning("Pivot point not assigned for SpinningFireStick.");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.Hit();
            }
        }
    }
}

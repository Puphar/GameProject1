using System.Collections;
using UnityEngine;

public class BlockItem : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        CircleCollider2D phyiscsCollider = GetComponent<CircleCollider2D>();
        BoxCollider2D triggerCollier = GetComponent<BoxCollider2D>();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        rigidbody.isKinematic = true;
        phyiscsCollider.enabled = false;
        triggerCollier.enabled = false;
        spriteRenderer.enabled = false;

        yield return new WaitForSeconds(0.25f);

        spriteRenderer.enabled = true;

        float elapsed = 0f;
        float duration = 0.5f;

        Vector3 startPosition = transform.localPosition;
        Vector3 endPosition = transform.localPosition + Vector3.up;

        while(elapsed < duration)
        {
            float t = elapsed / duration;

            transform.localPosition = Vector3.Lerp(startPosition, endPosition, t);
            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = endPosition;

        rigidbody.isKinematic = false;
        phyiscsCollider.enabled = true;
        triggerCollier.enabled = true;
        spriteRenderer.enabled = true;

    }
}

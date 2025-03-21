using UnityEngine;
using TMPro;

public class Koopa : MonoBehaviour
{
    public Animator animator;
    public Sprite shellSprite;
    public Sprite normalKoopaSprite;
    public float shellSpeed = 12f;
    public Transform startPosition;
    public Transform endPosition;
    public float flyingSpeed = 5f;
    private Rigidbody2D rb;

    public ScoreData scoreData;
    public TextMeshProUGUI scoreText;
    public GameObject popUpScorePrefab;
    public TMP_Text popUptext;


    private Enemy enemy;

    private bool shelled;
    private bool pushed;
    public bool isFlying; // Flag to determine if the Koopa is flying

    private void Update()
    {
        animator = GetComponent<Animator>();

        if (isFlying)
        {
            GetComponent<Rigidbody2D>().isKinematic = false;
            transform.position = Vector2.Lerp(startPosition.position, endPosition.position, Mathf.PingPong(Time.time * flyingSpeed, 1f));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!shelled && collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (player.starpower)
            {
                enemy.Hit();
            }
            else if (collision.transform.DotTest(transform, Vector2.down))
            {
                if (!isFlying)
                {
                    EnterShell();
                }
                else
                    TransformToNormalKoopa();
            }
            else
            {
                player.Hit();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (shelled && other.CompareTag("Player"))
        {
            if (!pushed)
            {
                Vector2 direction = new Vector2(transform.position.x - other.transform.position.x, 0f);
                PushShell(direction);
            }
            else
            {
                Player player = other.GetComponent<Player>();

                if (player.starpower)
                {
                    enemy.Hit();
                }
                else
                {
                    player.Hit();
                }
            }
        }
        else if (!shelled && other.gameObject.layer == LayerMask.NameToLayer("Shell"))
        {
            KoopaHit();
        }
    }

    private void EnterShell()
    {
        animator.SetBool("IsStomped", true);
        shelled = true;

        GetComponent<SpriteRenderer>().sprite = shellSprite;
        GetComponent<EntityMovement>().enabled = false;
    }

    private void PushShell(Vector2 direction)
    {
        pushed = true;
        animator.SetBool("IsKicked", true);

        GetComponent<Rigidbody2D>().isKinematic = false;

        EntityMovement movement = GetComponent<EntityMovement>();
        movement.direction = direction.normalized;
        movement.speed = shellSpeed;
        movement.enabled = true;

        gameObject.layer = LayerMask.NameToLayer("Shell");
    }

    private void OnBecameInvisible()
    {
        if (pushed)
        {
            Destroy(gameObject);
        }
    }

    private void TransformToNormalKoopa()
    {
        isFlying = false;
        isFlying = false;
        GetComponent<EntityMovement>().enabled = true;
        GetComponent<SpriteRenderer>().sprite = normalKoopaSprite;
        GetComponent<Collider2D>().enabled = true;
    }

    private void KoopaHit()
    {
        GetComponent<DeathAnimation>().enabled = true;
        Destroy(gameObject, 3f);

        popUptext.text = "100";
        Instantiate(popUpScorePrefab, transform.position, Quaternion.identity);

        scoreData.scoreValue += 100;
        scoreText.text = " " + scoreData.scoreValue;
        Debug.Log("Add score");
    }
}



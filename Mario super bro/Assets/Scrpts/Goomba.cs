using UnityEngine;
using TMPro;

public class Goomba : MonoBehaviour
{
    public Sprite flatSprite;
    public ScoreData scoreData;
    public TextMeshProUGUI scoreText;
    public GameObject popUpScorePrefab;
    public TMP_Text popUptext;

    private Enemy enemy;

    public Animator animator;

    private void Start()
    {
        scoreText.text = " " + scoreData.scoreValue;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player.starpower)
            {
                enemy.Hit();
            }
            else if (collision.transform.DotTest(transform, Vector2.down))
            {
                Flatten();
            }
            else
            {
                player.Hit();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Shell"))
        {
            GoomHit();
        }
    }

    private void Flatten()
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<EntityMovement>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = flatSprite;
        GetComponent<Animator>().SetBool("IsStomp", true);
        
        popUptext.text = "100";
        Instantiate(popUpScorePrefab, transform.position, Quaternion.identity);

        scoreData.scoreValue += 100;
        scoreText.text = " " + scoreData.scoreValue;
        Debug.Log("Add score");

        Destroy(gameObject, 0.5f);
    }

    private void GoomHit()
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

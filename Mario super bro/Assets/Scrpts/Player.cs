using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator smallMario;
    public Animator bigMario;
    public Animator fireMario;

    public GameObject marioSmall;
    public GameObject marioBig;
    public GameObject marioFire;

    public Collider2D playerCollider;

    public Animator currentanim;
    public GameObject currentGO;


    private PlayerMovement playerMovement;

    public float damageCooldown = 2f; // Adjust this value as needed

    private bool canTakeDamage = true;

    public GameObject fireballPrefab;
    public Transform fireballSpawnPoint;
    public float fireballSpeed = 10f;

    public bool starpower = false;
    public bool flower = false;
    public bool big = false;
    public bool small = true;


    private void Awake()
    {
        playerCollider = GetComponent<Collider2D>();
        currentanim = smallMario;
        currentGO = marioSmall;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1)) // Change KeyCode.Space to the desired input key
        {
            ShootFireball();
            Debug.Log("Shoot fire");
        }
    }

    public void Hit()
    {
        if (canTakeDamage)
        {
            if(currentGO == marioFire)
            {
                Shrink();
                StartCoroutine(DamageCooldown());
                Debug.Log("Shrink");
            }
            else if (currentGO == marioBig)
            {
                Shrink();
                StartCoroutine(DamageCooldown());
                Debug.Log("Shrink");
            }
            else if (currentGO == marioSmall)
            {
                Death();
                Debug.Log("Death");
            }
        }
    }

    private IEnumerator DamageCooldown()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canTakeDamage = true;
    }

    private void Death()
    {
        smallMario.SetBool("IsDead", true);
        //playerMovement.enabled = false;
        playerCollider.enabled = false;


        GameManager.Instance.ResetLevel(3f);
    }

    public void Fireflower()
    {
        if (small)
        {
            Grow();
        }
        else if (big)
        {
            Fire();
        }
    }

    public void Fire()
    {
        fireMario.Play("Mario_TransformToFire");

        smallMario.enabled = false;
        bigMario.enabled = false;
        fireMario.enabled = true;

        marioFire.SetActive(true);
        marioBig.SetActive(false);
        marioSmall.SetActive(false);

        currentanim = fireMario;
        currentGO = marioFire;

    }

    public void Grow()
    {
        bigMario.Play("SmallMario_TransfromToBig");

        smallMario.enabled = false;
        bigMario.enabled = true;
        fireMario.enabled = false;

        marioFire.SetActive(false);
        marioBig.SetActive(true);
        marioSmall.SetActive(false);

        currentanim = bigMario;
        currentGO = marioBig;

    }

    private void Shrink()
    {
        smallMario.Play("Mario_TransformToSmall");

        smallMario.enabled = true;
        bigMario.enabled = false;
        fireMario.enabled = false;

        marioFire.SetActive(false);
        marioBig.SetActive(false);
        marioSmall.SetActive(true);

        currentanim = smallMario;
        currentGO = marioSmall;

    }

    public void Starpower(float duration = 10f)
    {
        StartCoroutine(StarpowerAnimation(duration));
    }

    private IEnumerator StarpowerAnimation(float duration)
    {
        starpower = true;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            if (Time.frameCount % 4 == 0)
            {
                //activeRenderer.spriteRenderer.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
            }

            yield return null;
        }

        //activeRenderer.spriteRenderer.color = Color.red;
        starpower = false;
    }

    public void ShootFireball()
    {
        if (fireMario.enabled)
        {
            // Instantiate a fireball prefab at the fireball spawn point
            GameObject fireball = Instantiate(fireballPrefab, fireballSpawnPoint.position, fireballSpawnPoint.rotation);

            // Get the Rigidbody2D component of the fireball
            Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();

            // Set the velocity of the fireball to shoot it in the forward direction
            rb.velocity = transform.right * fireballSpeed;

            // Optionally, play sound effects or visual effects for firing a fireball
        }
    }
}

using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;

    private Player player;
    private Animator animator;
    private GameObject playerGO;

    private new Camera camera;
    private new Rigidbody2D rigidbody;
    [SerializeField] Transform groundCheckCollieder;
    [SerializeField] LayerMask groundLayer;
    public CapsuleCollider2D capsuleCollider;
    public CircleCollider2D circleCollider;


    const float groundRadius = 0.2f;

    public Vector2 velocity;
    private float inputAxis;

    bool crouch;

    public float moveSpeed = 8f;
    public float runSpeed = 100f;
    public float maxJumpHeight = 5f;
    public float maxJumpTime = 1f;
    private bool resetMaxJumpHeight = false;

    public float jumpForce => (2f * maxJumpHeight) / (maxJumpTime / 2f);
    public float gravity => (-2f * maxJumpHeight) / Mathf.Pow((maxJumpTime / 2f), 2);

    public bool grounded { get; private set; }
    public bool jumping { get; private set; }
    public bool running => Mathf.Abs(velocity.x) > 0.25f || Mathf.Abs(inputAxis) > 0.25f;
    public bool sliding => (inputAxis > 0 && velocity.x < 0f) || (inputAxis < 0 && velocity.x > 0f);

    public bool flippedLeft;
    public bool facingRight;

    public float jumpBoostForce = 30f;
    public float jumpBoostDuration = 2f;
    public float boostedMaxJumpHeight = 30f;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        camera = Camera.main;
        player = GetComponent<Player>();
        animator = player.currentanim;
        playerGO = player.currentGO;
        player.currentGO.SetActive(true);
    }
        
    private void Update()
    {
        HorizontalMovement();

        GroundCheck();

        if (grounded)
        {
            GroundedMovement();
        }

        ApplyGravity();
    }


    private void HorizontalMovement()
    {
        inputAxis = Input.GetAxis("Horizontal");
        velocity.x = Mathf.MoveTowards(velocity.x, inputAxis * moveSpeed, moveSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveSpeed = 10f;

        }
        else if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = 5f;
        }

        if (rigidbody.Raycast(Vector2.right * velocity.x))
        {
            velocity.x = 0f;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Crouch();
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            StandUp();
        }

        if(velocity.x > 0)
        {
            facingRight = true;
            Flip(true);
            transform.eulerAngles = Vector3.zero;
        }
        else if (velocity.x < 0)
        {
            facingRight = false;
            Flip(false);
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }

        if (velocity.x == 0f)
        {
            player.currentanim.SetBool("IsWalking", false); // Reset other animations
            player.currentanim.SetBool("IsRunning", false); // Reset other animations
        }
        else if (velocity.x <= 8f)
        {
            player.currentanim.SetBool("IsWalking", true);
            player.currentanim.SetBool("IsRunning", false); // Reset other animations
        }
        else if (velocity.x > 8f)
        {
            player.currentanim.SetBool("IsWalking", false); // Reset other animations
            player.currentanim.SetBool("IsRunning", true);
        }
    }  

    public void GroundedMovement()
    {
        velocity.y = Mathf.Max(velocity.y, 0f);
        jumping = velocity.y > 0f;

        // Player jumps
        if (Input.GetButtonDown("Jump") && grounded)
        {
            velocity.y = jumpForce;
            jumping = true;
            grounded = false;
            player.currentanim.SetBool("IsJumping", true);
        }
        else if (Input.GetKeyUp(KeyCode.Space) && !grounded) 
        {
            velocity.y = 0;
            jumping = false;
            player.currentanim.SetBool("IsJumping", false);
        }
        else if (grounded && velocity.y <= 0)
        {
            player.currentanim.SetBool("IsJumping", false);
        }
    }

    void GroundCheck()
    {
        grounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollieder.position, groundRadius, groundLayer);
        if (colliders.Length > 0)
        {
            grounded = true;
        }
    }

    public void ApplyGravity()
    {
        bool falling = velocity.y < 0f || !Input.GetButton("Jump");
        float multiplier = falling ? 2f : 1f;

        velocity.y += gravity * multiplier * Time.deltaTime;
        velocity.y = Mathf.Max(velocity.y, gravity / 2f);
    }

    private void FixedUpdate()
    {
        Vector2 position = rigidbody.position;
        position += velocity * Time.deltaTime;

        rigidbody.MovePosition(position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if(transform.DotTest(collision.transform, Vector2.down))
            {
                velocity.y = jumpForce / 2f;
                jumping = true;
            }
        }

        if(collision.gameObject.layer != LayerMask.NameToLayer("Power up"))
        {
            if(transform.DotTest(collision.transform, Vector2.up))
            {
                velocity.y = 0f;
            }
        }

        if (collision.gameObject.CompareTag("JumpPad"))
        {
            JumpBoost();
            maxJumpHeight = 10;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("JumpPad"))
        {
            maxJumpHeight = 10f; // Set max jump height to 10f temporarily
            resetMaxJumpHeight = true; // Set the flag to indicate that we need to reset maxJumpHeight
            StartCoroutine(ResetMaxJumpHeightAfterDelay()); // Start the coroutine to reset maxJumpHeight after delay
        }
    }
    void Flip(bool facingRight)
    {
        if(flippedLeft && facingRight)
        {
            transform.Rotate(0, -180, 0);
            flippedLeft = false;
        }
        if(!flippedLeft && !facingRight)
        {
            transform.Rotate(0, -180, 0);
            flippedLeft = true;
        }
    }

    private void JumpBoost()
    {
        Rigidbody2D playerRigidbody = GetComponent<Rigidbody2D>();
        if (playerRigidbody != null)
        {
            // Apply the jump boost force to the player
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, jumpBoostForce);

            // Optional: Change the max jump height temporarily
            float originalMaxJumpHeight = maxJumpHeight;
            maxJumpHeight = boostedMaxJumpHeight;

            // Start a coroutine to reset the max jump height after a certain duration
            StartCoroutine(ResetMaxJumpHeight(originalMaxJumpHeight));
        }
    }

    private IEnumerator ResetMaxJumpHeight(float originalMaxJumpHeight)
    {
        // Wait for the duration of the jump boost
        yield return new WaitForSeconds(jumpBoostDuration);

        // Reset the max jump height to its original value
        maxJumpHeight = originalMaxJumpHeight;
    }

    public void Crouch()
    {
        crouch = true;
        capsuleCollider.enabled = false;
        circleCollider.enabled = true;
        player.currentanim.SetBool("IsCrouch", true);
    }

    public void StandUp()
    {
        crouch = false;
        capsuleCollider.enabled = true;
        circleCollider.enabled = false;
        player.currentanim.SetBool("IsCrouch", false);
    }

    IEnumerator ResetMaxJumpHeightAfterDelay()
    {
        yield return new WaitForSeconds(3f); // Wait for 3 seconds

        if (resetMaxJumpHeight)
        {
            maxJumpHeight = 5f; // Reset max jump height back to 5f
            resetMaxJumpHeight = false; // Reset the flag
        }
    }
}

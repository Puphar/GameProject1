using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    public Animator animator;
    private PlayerMovement movement;
    [SerializeField] LayerMask groundLayer;

    private float previousHorizontalInput;
    private bool isGrounded;

    private void Start()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        HandleAnimations();
    }

    private void HandleAnimations()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        // Set the "IsWalking" parameter based on player input
        animator.SetBool("IsWalking", Mathf.Abs(horizontalInput) > 0.1f);

        // Set the "IsRunning" parameter based on LeftShift key
        animator.SetBool("IsRunning", Input.GetKey(KeyCode.LeftShift));


        // Set the "IsSliding" parameter based on change of direction
        animator.SetBool("IsSliding", horizontalInput * previousHorizontalInput < 0);

        previousHorizontalInput = horizontalInput;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
            animator.SetBool("IsJumping", false); // Ensure the "IsJumping" animation is stopped when grounded
            Debug.Log("Ground");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = false;
            Debug.Log("Left Ground");
        }
    }
}

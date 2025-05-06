using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Tooltip("The speed at which the player moves.")]
    public float moveSpeed = 5f;

    [Tooltip("Force of the jump.")]
    public float jumpForce = 5f;

    public LayerMask groundLayer;                                                        // Layer mask to check for ground collision
    public Transform groundCheck;                                                        // Transform to check if player is grounded

    public float groundCheckRadius = 0.2f;                                               // Radius of the ground check sphere

    private Rigidbody rb;
    private PlayerInputHandler input;
    private Animator animator;
    private bool isGrounded;                                                            // Boolean to check if player is grounded

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();                                                     // Get the Ridgidbody, input handler, and animator components
        input = GetComponent<PlayerInputHandler>();                                         // <<
        animator = GetComponent<Animator>();                                                // <<
    }   

    private void FixedUpdate()
    {
        Vector2 move = input.MoveInput;                                                     // Get current input from WASD or left stick
        Vector3 moveDirection = new Vector3(move.x, 0f, move.y);                            // Convert 2D input to 3D direction

        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);     // Apply movement to the Rigidbody
                                       
        animator.SetFloat("Speed", moveDirection.magnitude);

        // ground check using raycast
        isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, out RaycastHit hit, 0.3f);

        if (input.JumpInput && isGrounded)                                 // Check if jump input is pressed and player is grounded
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);                          // Apply jump force to the Rigidbody
            animator.SetTrigger("Jump");                                                      // Trigger jump animation
        }

        Debug.DrawRay(rb.position, moveDirection * 3f, Color.blue);                         // Debug: Draw a ray in the direction of movement
    }
}

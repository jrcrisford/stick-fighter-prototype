using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("The speed at which the player moves.")]
    public float moveSpeed = 5f;

    [Header("Jump Settings")]
    [Tooltip("Force applied upward when jumping.")]
    public float jumpForce = 5f;

    [Header("Ground Detection")]
    [Tooltip("The ground layer used for detecting if the player is grounded.")]
    public LayerMask groundLayer;

    [Tooltip("Transform used as the origin for ground checking.")]
    public Transform groundCheck;

    [Tooltip("Radius of the sphere used for checking if the player is grounded.")]
    public float groundCheckRadius = 0.2f;

    private Rigidbody rb;
    private PlayerInputHandler input;
    private Animator animator;

    private bool isGrounded;                                // Flag to check if player is grounded
    private bool canJump = true;                            // Flag to check if player can jump

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();                     // Get the Ridgidbody, input handler, and animator components
        input = GetComponent<PlayerInputHandler>();         // <<
        animator = GetComponent<Animator>();                // <<
    }

    private void Update()
    {
        // Update the animator params each frame to sync with rendering
        animator.SetFloat("Speed", input.MoveInput.magnitude);
        animator.SetBool("isGrounded", isGrounded);
    }

    private void FixedUpdate()
    {
        // Convert 2D input into a 3D direction vector
        Vector2 move = input.MoveInput;                                                   
        Vector3 moveDirection = new Vector3(move.x, 0f, move.y);                          

        // Apply movement to the Rigidbody
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);     

        // Check for ground using a raycast
        isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, out RaycastHit hit, 0.3f);

        // Check if jump input is pressed and player is grounded
        if (input.JumpInput && isGrounded && canJump)                                       
        {
            StartCoroutine(PerformJump());         
        }

        // Debug: Draw a ray in the direction of movement
        Debug.DrawRay(rb.position, moveDirection * 3f, Color.blue);                         
    }

    /// <summary>
    /// Executes jump, including the animation trigger and cooldown.
    /// </summary>
    private IEnumerator PerformJump()
    {
        // Disable jumping until cooldown is over
        canJump = false;  
        
        // Trigger jump animation
        animator.SetTrigger("Jump");

        // Small delay to allow animation anticipation before jump force
        yield return new WaitForSeconds(0.3f);

        // Apply jump force
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        // Wait for cooldown before re-enabling jumping
        yield return new WaitForSeconds(0.5f);                                  
        canJump = true;                                                            
    }
}

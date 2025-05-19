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
        rb.isKinematic = true;
        input = GetComponent<PlayerInputHandler>();         // <<
        animator = GetComponent<Animator>();                // <<
    }

    private void Update()
    {
        // Check for ground using a raycast
        isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, out RaycastHit hit, 0.3f);

        // Update the animator params each frame to sync with rendering
        animator.SetFloat("Speed", input.MoveInput.magnitude);
        animator.SetBool("isGrounded", isGrounded);

        Debug.Log($"Jump input: {input.JumpInput}, Grounded: {isGrounded}, CanJump: {canJump}");


        if (input.JumpInput && isGrounded && canJump)
        {
            StartCoroutine(PerformJump());
        }
    }

    private void FixedUpdate()
    {
        // Convert 2D input into a 3D direction vector
        Vector2 move = input.MoveInput;                                                   
        Vector3 moveDirection = new Vector3(move.x, 0f, move.y);

        // Apply movement to the Rigidbody
        transform.position += moveDirection * moveSpeed * Time.fixedDeltaTime;

        // Debug: Draw a ray in the direction of movement
        Debug.DrawRay(rb.position, moveDirection * 3f, Color.blue);                         
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Death Plane"))
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Executes jump, including the animation trigger and cooldown.
    /// </summary>
    private IEnumerator PerformJump()
    {
        canJump = false;
        animator.SetTrigger("Jump");

        // Wait for animation wind-up before jumping
        yield return new WaitForSeconds(0.3f);

        float jumpHeight = 2.5f;
        float jumpDuration = 0.8f;

        float elapsed = 0f;
        Vector3 start = transform.position;
        Vector3 peak = start + Vector3.up * jumpHeight;

        // Jump up
        while (elapsed < jumpDuration / 2f)
        {
            float t = elapsed / (jumpDuration / 2f);
            transform.position = Vector3.Lerp(start, peak, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Jump down
        elapsed = 0f;
        Vector3 landing = start;
        while (elapsed < jumpDuration / 2f)
        {
            float t = elapsed / (jumpDuration / 2f);
            transform.position = Vector3.Lerp(peak, landing, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);
        canJump = true;
    }

}

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Tooltip("The speed at which the player moves.")]
    public float moveSpeed = 5f;

    private Rigidbody rb;
    private PlayerInputHandler input;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();                                                     // Get the Ridgidbody and input handler components
        input = GetComponent<PlayerInputHandler>();                                         // <<
    }   

    private void FixedUpdate()
    {
        Vector2 move = input.MoveInput;                                                     // Get current input from WASD or left stick
        Vector3 moveDirection = new Vector3(move.x, 0f, move.y);                            // Convert 2D input to 3D direction

        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);     // Apply movement to the Rigidbody

        Debug.DrawRay(rb.position, moveDirection * 3f, Color.blue);                         // Debug: Draw a ray in the direction of movement
        Debug.Log("Move Direction: " + moveDirection);                                      // Debug: Log the move direction
    }
}

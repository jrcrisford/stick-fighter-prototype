using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAiming : MonoBehaviour
{
    [Tooltip("Speed at which the player rotates to face the target point.")]
    public float rotationSpeed = 10f;

    /* For implementing controller aiming
    private PlayerInputHandler input;

    private void Awake()
    {
        input = GetComponent<PlayerInputHandler>();
    }
    */

    private void Update()
    {
        Vector2 mousePos = Input.mousePosition;                                 // Get mouse position
        Ray ray = Camera.main.ScreenPointToRay(mousePos);                       // Create a ray from the camera to the mouse position

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))                     // Check if raycast hits an object
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red);                   // Debug: draw the ray from camera
            Debug.DrawLine(transform.position, hit.point, Color.green);         // Debug: draw a line from the player to the hit point
            Debug.Log("Raycast hit at: " + hit.point);

            Vector3 targetPoint = hit.point;                                    // Flatten target point to player height and rotate toward it
            targetPoint.y = transform.position.y;                               // <<
            RotateToward(targetPoint);                                          // <<
        } 
        else { Debug.Log("Raycast did not hit anything."); } 
    }

    // Performs roation of the player to face a target point
    private void RotateToward(Vector3 targetPoint)
    {
        Vector3 direction = targetPoint - transform.position;                   // Calculate direction to target point
        if (direction.sqrMagnitude > 0.01f)                                     // Prevent jittering                        
        {                                                               
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}


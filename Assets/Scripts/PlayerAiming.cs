using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAiming : MonoBehaviour
{
    public float rotationSpeed = 10f;

    private PlayerInputHandler input;

    private void Awake()
    {
        input = GetComponent<PlayerInputHandler>();
    }

    private void Update()
    {
        Vector2 mousePos = Input.mousePosition;

        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red);
            Debug.DrawLine(transform.position, hit.point, Color.green);
            Debug.Log("Raycast hit at: " + hit.point);

            Vector3 targetPoint = hit.point;
            targetPoint.y = transform.position.y;
            RotateToward(targetPoint);
        }
        else
        {
            Debug.Log("Raycast did not hit anything.");
        }
        
    }

    private void RotateToward(Vector3 targetPoint)
    {
        Vector3 direction = targetPoint - transform.position;
        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}


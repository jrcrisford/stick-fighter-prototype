using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Tooltip("Target the camera follows")]
    public Transform target;

    [Tooltip("Offset from the target position")]
    public Vector3 offset = new Vector3(0f, 10f, -10f);

    [Tooltip("How quickly the camera moves to follow")]
    public float followSpeed = 5f;

    private void LateUpdate()
    {
        if (target == null) return;                                                                 // Stop running if no target is defined

        Vector3 desiredPosition = target.position + offset;                                         // Calulate the desired position with offset
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed);        // Smoothly move camera toward desired position
        transform.LookAt(target);                                                                   // Alway look a the target

        Debug.DrawLine(transform.position, target.position, Color.cyan);                            // Debug: Line from camera to target 
    }
}

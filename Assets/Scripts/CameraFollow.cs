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
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed);   
        transform.LookAt(target);
    }
}

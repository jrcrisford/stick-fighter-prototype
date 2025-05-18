using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform cam;

    private void Awake()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            cam = mainCamera.transform;
        }
        else
        {
            Debug.LogWarning("Billboard: Main Camera not found.");
        }
    }

    private void LateUpdate()
    {
        if (cam != null)
        {
            transform.LookAt(transform.position + cam.forward);
        }
    }
}
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] Transform target; // Drag your Rocket here
    [SerializeField] Vector3 offset = new Vector3(0, 5, -10); // Position relative to rocket

    [Header("Smoothness")]
    [SerializeField] float smoothSpeed = 0.125f;

    void LateUpdate()
    {
        if (target == null) return;

        // Desired position based on the rocket's current position + our offset
        Vector3 desiredPosition = target.position + offset;

        // Smoothly interpolate between current position and desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Update camera position
        transform.position = smoothedPosition;

        // Optional: Keep the camera looking at the rocket
        transform.LookAt(target);
    }
}
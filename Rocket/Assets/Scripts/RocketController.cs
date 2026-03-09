using UnityEngine;
using UnityEngine.InputSystem; // Required for the New Input System

public class RocketController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] float thrustForce = 1200f;
    [SerializeField] float rotationSpeed = 120f;

    [Header("Descent Settings")]
    [Tooltip("How strongly the rocket counteracts gravity when landing")]
    [SerializeField] float landingCushionForce = 1600f;

    [Header("Effects")]
    [SerializeField] ParticleSystem thrustParticles;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Settings these via code ensures a consistent "heavy but smooth" feel
        rb.linearDamping = 1.5f;
        rb.angularDamping = 2.0f;
    }

    void FixedUpdate()
    {
        HandleThrust();
        HandleRotation();
    }

    void HandleThrust()
    {
        // MAIN THRUSTER: Space to ascend
        if (Keyboard.current.spaceKey.isPressed)
        {
            rb.AddRelativeForce(Vector3.up * thrustForce * Time.fixedDeltaTime);

            if (thrustParticles != null && !thrustParticles.isPlaying)
                thrustParticles.Play();
        }
        else
        {
            if (thrustParticles != null && thrustParticles.isPlaying)
                thrustParticles.Stop();
        }

        // CONTROLLED DESCENT: Shift to land smoothly
        if (Keyboard.current.leftShiftKey.isPressed)
        {
            if (rb.linearVelocity.y < -0.1f)
            {
                rb.AddForce(Vector3.up * landingCushionForce * Time.fixedDeltaTime);
            }
            else
            {
                rb.AddRelativeForce(Vector3.down * (thrustForce * 0.2f) * Time.fixedDeltaTime);
            }
        }
    }

    void HandleRotation()
    {
        float pitch = 0f; // Tilt forward/back (X-axis)
        float yaw = 0f;   // Tilt side-to-side (Z-axis)
        float roll = 0f;  // Spin (Y-axis)

        // PITCH: W/S
        if (Keyboard.current.wKey.isPressed) pitch = 1f;
        else if (Keyboard.current.sKey.isPressed) pitch = -1f;

        // YAW: A/D (Turning left/right)
        if (Keyboard.current.aKey.isPressed) yaw = 1f;
        else if (Keyboard.current.dKey.isPressed) yaw = -1f;

        // ROLL: Q/E (Spinning like a screwdriver)
        if (Keyboard.current.qKey.isPressed) roll = 1f;
        else if (Keyboard.current.eKey.isPressed) roll = -1f;

        // Apply forces to the local axes
        rb.AddRelativeTorque(Vector3.right * pitch * rotationSpeed * Time.fixedDeltaTime);
        rb.AddRelativeTorque(Vector3.forward * yaw * rotationSpeed * Time.fixedDeltaTime);
        rb.AddRelativeTorque(Vector3.up * roll * rotationSpeed * Time.fixedDeltaTime);
    }
}
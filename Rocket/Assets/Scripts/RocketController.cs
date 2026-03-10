using UnityEngine;
using UnityEngine.InputSystem; // Required for the New Input System

public class RocketController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] float thrustForce = 1200f;
    [SerializeField] float rotationSpeed = 150f;

    [Header("Descent Settings")]
    [Tooltip("How strongly the rocket counteracts gravity when landing")]
    [SerializeField] float landingCushionForce = 1600f;

    [Header("Effects")]
    [SerializeField] ParticleSystem thrustParticles;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Fix for CS1061: Using 'drag' and 'angularDrag' for compatibility
        rb.linearDamping = 1.5f;
        rb.angularDamping = 2.0f;

        // Ensure the rocket starts on the 2D gameplay plane (Z = 0)
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    void FixedUpdate()
    {
        HandleThrust();
        Handle2DRotation();
    }

    void HandleThrust()
    {
        // MAIN THRUSTER: Space to ascend
        if (Keyboard.current.spaceKey.isPressed)
        {
            rb.AddRelativeForce(Vector3.up * thrustForce * Time.fixedDeltaTime);

            // Instant Particle Trigger
            if (thrustParticles != null && !thrustParticles.isPlaying)
            {
                thrustParticles.Clear(); // Removes old particles to prevent restart delay
                thrustParticles.Play();
            }
        }
        else
        {
            // Stop flames when Space is released
            if (thrustParticles != null && thrustParticles.isPlaying)
            {
                thrustParticles.Stop();
            }
        }

        // CONTROLLED DESCENT: Shift to land smoothly
        // Checks if falling (negative velocity) to apply the cushion
        if (Keyboard.current.leftShiftKey.isPressed && rb.linearVelocity.y < -0.1f)
        {
            rb.AddForce(Vector3.up * landingCushionForce * Time.fixedDeltaTime);
        }
    }

    void Handle2DRotation()
    {
        float tilt = 0f;

        // In a 2.5D environment, we only tilt on the Z-axis (left/right)
        if (Keyboard.current.aKey.isPressed) tilt = 1f;
        else if (Keyboard.current.dKey.isPressed) tilt = -1f;

        // Apply torque to the Z-axis (Vector3.forward)
        rb.AddRelativeTorque(Vector3.forward * tilt * rotationSpeed * Time.fixedDeltaTime);
    }
}
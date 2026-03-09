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

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Settings these via code ensures the "smooth" feel every time
        rb.linearDrag = 1.5f;
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
        }

        // CONTROLLED DESCENT: Shift to land smoothly
        if (Keyboard.current.leftShiftKey.isPressed)
        {
            // If the rocket is falling (negative Y velocity), apply upward force to cushion
            if (rb.linearVelocity.y < -0.1f)
            {
                // We use Vector3.up (World Space) to directly fight gravity
                rb.AddForce(Vector3.up * landingCushionForce * Time.fixedDeltaTime);
            }
            else
            {
                // If we are hovering or moving up, give a tiny nudge down for precision
                rb.AddRelativeForce(Vector3.down * (thrustForce * 0.2f) * Time.fixedDeltaTime);
            }
        }
    }

    void HandleRotation()
    {
        float pitch = 0f;
        float roll = 0f;

        // W/S for Pitch
        if (Keyboard.current.wKey.isPressed) pitch = 1f;
        else if (Keyboard.current.sKey.isPressed) pitch = -1f;

        // A/D for Roll
        if (Keyboard.current.aKey.isPressed) roll = 1f;
        else if (Keyboard.current.dKey.isPressed) roll = -1f;

        rb.AddRelativeTorque(Vector3.right * pitch * rotationSpeed * Time.fixedDeltaTime);
        rb.AddRelativeTorque(Vector3.forward * roll * rotationSpeed * Time.fixedDeltaTime);
    }
}
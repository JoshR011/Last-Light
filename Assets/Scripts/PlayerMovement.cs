/*
 * Authors: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song
 * Last updated: 2026-10-19
 * Summary: Handles Rigidbody movement, grounded jumping, and first-person camera look.
 */

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float speed;

    [Header("Horizontal Look")]
    public float rotationSpeed;

    [Header("Jumping")]
    public float jumpForce;

    [Header("Vertical Look")]
    public Transform cameraTransform;
    public float verticalLookSpeed;

    private const float MaximumPitchDegrees = 80f;
    private const float GroundCheckMargin = 0.1f;
    private const float MinimumGroundNormalY = 0.5f;
    private Vector2 movementForce;
    private float yawTorque;
    private float pitchInput;
    private float cameraPitchDegrees;
    private Rigidbody playerRigidbody;
    private Collider playerCollider;

    private void Awake()
    {
        // Locking the pointer lets mouse-look continue when the mouse reaches the screen edge.
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Cache these components once because movement and ground checks reuse them throughout play.
        playerRigidbody = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();
    }

    public void OnMove(InputValue value)
    {
        // Retain the input between callbacks so holding a direction continues to apply movement.
        movementForce = value.Get<Vector2>() * speed;
    }

    public void OnLook(InputValue value)
    {
        // Separate yaw from pitch so looking upward does not tilt the player's physics body.
        Vector2 lookInput = value.Get<Vector2>();
        yawTorque = lookInput.x * rotationSpeed;
        pitchInput = lookInput.y * verticalLookSpeed;
    }

    public void OnJump(InputValue value)
    {
        // Ignore button releases and upward motion so the same jump cannot receive another boost.
        if (!value.isPressed || playerRigidbody.velocity.y > 0.1f)
        {
            return;
        }

        if (IsGrounded())
        {
            // An impulse gives each jump a single boost instead of an ongoing upward force.
            playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private bool IsGrounded()
    {
        // Collider bounds keep the check aligned with the player's actual size and offset.
        Bounds bounds = playerCollider.bounds;

        // Pickups must not count as ground, and steep walls must not permit jumping.
        return Physics.Raycast(
            bounds.center, Vector3.down, out RaycastHit hit,
            bounds.extents.y + GroundCheckMargin, Physics.DefaultRaycastLayers,
            QueryTriggerInteraction.Ignore) && hit.normal.y >= MinimumGroundNormalY;
    }

    private void Update()
    {
        ApplyMovement();
        ApplyBodyRotation();
        UpdateCameraPitch();
    }

    private void ApplyMovement()
    {
        // Local axes make forward input follow the player's facing direction after turning.
        playerRigidbody.AddRelativeForce(
            movementForce.x * Time.deltaTime,
            0,
            movementForce.y * Time.deltaTime);
    }

    private void ApplyBodyRotation()
    {
        // Turning the body keeps movement and the attached camera facing the same horizontal direction.
        playerRigidbody.AddRelativeTorque(0, yawTorque * Time.deltaTime, 0);
    }

    private void UpdateCameraPitch()
    {
        // Subtract vertical input so moving the mouse upward makes the camera look upward.
        cameraPitchDegrees -= pitchInput * Time.deltaTime;
        // Limit pitch so the camera cannot flip upside down.
        cameraPitchDegrees = Mathf.Clamp(cameraPitchDegrees, -MaximumPitchDegrees, MaximumPitchDegrees);
        cameraTransform.localRotation = Quaternion.Euler(cameraPitchDegrees, 0, 0);
    }
}

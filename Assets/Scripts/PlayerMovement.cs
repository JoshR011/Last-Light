/*
 * Authors: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song
 * Date Created: 2026-10-07
 * Date Last Updated: 2026-11-20
 * Summary: Handles Rigidbody movement, grounded jumping, and first-person camera look.
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float speed;
    [Min(0f)]
    public float horizontalDrag = 2f;
    [Min(0f)]
    public float groundBrakingDeceleration = 80f;

    [Header("Horizontal Look")]
    public float rotationSpeed;

    [Header("Jumping")]
    public float jumpForce = 5f;
    [Min(0f)]
    public float jumpBufferSeconds = 0.15f;
    [Min(0f)]
    public float coyoteTimeSeconds = 0.1f;

    [Header("Vertical Look")]
    public Transform cameraTransform;
    public float verticalLookSpeed;

    [Header("Footstep Audio")]
    public AudioSource footstepSound;
    public AudioClip grassWalk;
    public AudioClip grassJump;
    public AudioClip grassLand;
    public AudioClip woodWalk;
    public AudioClip woodJump;
    public AudioClip woodLand;

    private const float MaximumPitchDegrees = 80f;
    private const float MinimumGroundNormalY = 0.5f;
    private readonly HashSet<Collider> groundContacts = new HashSet<Collider>();
    private Vector2 movementForce;
    private float yawInput;
    private float pendingYawDegrees;
    private float pitchInput;
    private float cameraPitchDegrees;
    private bool jumpRequested;
    private float jumpBufferExpiresAt = float.NegativeInfinity;
    private float lastGroundedTime = float.NegativeInfinity;
    private bool waitingForGroundSeparation;
    private Rigidbody playerRigidbody;
    private Collider playerCollider;
    private PhysicMaterial movementMaterial;
    private PlayerCrouchSlide crouchSlide;
    private float stepTimer;
    private bool waitingForJumpLanding;
    private bool jumpClearedGround;

    private void Awake()
    {
        // Locking the pointer lets mouse-look continue when the mouse reaches the screen edge.
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Cache these components once because movement and ground checks reuse them throughout play.
        playerRigidbody = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();
        crouchSlide = GetComponent<PlayerCrouchSlide>();
        ConfigureMovementPhysics();
    }

    private void ConfigureMovementPhysics()
    {
        // Wall friction can counter gravity while movement pushes the capsule into a wall.
        // Use a private material so every player instance slides without changing world materials.
        movementMaterial = new PhysicMaterial("Player Movement")
        {
            staticFriction = 0f,
            dynamicFriction = 0f,
            frictionCombine = PhysicMaterialCombine.Minimum,
            bounciness = 0f,
            bounceCombine = PhysicMaterialCombine.Minimum
        };
        playerCollider.sharedMaterial = movementMaterial;

        // Apply drag horizontally ourselves so it cannot slow the player's fall.
        playerRigidbody.drag = 0f;

        // Contacts cannot turn the body; mouse look explicitly controls its rotation.
        playerRigidbody.constraints |= RigidbodyConstraints.FreezeRotation;
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
        yawInput = lookInput.x * rotationSpeed;
        pitchInput = lookInput.y * verticalLookSpeed;
    }

    public void OnJump(InputValue value)
    {
        // Retain a press until the next physics step, ignoring button releases.
        if (value.isPressed)
        {
            jumpRequested = true;
        }
    }

    private void ApplyJump()
    {
        if (jumpRequested)
        {
            jumpRequested = false;
            jumpBufferExpiresAt = Time.time + jumpBufferSeconds;
        }

        bool grounded = IsGrounded();
        if (waitingForGroundSeparation)
        {
            // The launch contact can survive for a physics step. Do not rearm from it.
            // A blocked jump can rearm once upward motion stops, even under a low ceiling.
            if (!grounded || playerRigidbody.velocity.y <= 0f)
            {
                waitingForGroundSeparation = false;
            }
            else
            {
                grounded = false;
            }
        }

        if (grounded)
        {
            lastGroundedTime = Time.time;
        }

        // Briefly retain presses before landing and ground support after walking off an edge.
        if (Time.time > jumpBufferExpiresAt || Time.time - lastGroundedTime > coyoteTimeSeconds)
        {
            return;
        }

        // Consume both windows so neither coyote time nor a lingering contact permits a second jump.
        jumpBufferExpiresAt = float.NegativeInfinity;
        lastGroundedTime = float.NegativeInfinity;
        waitingForGroundSeparation = true;

        // Starting from the same vertical speed keeps slopes and falling from weakening the jump.
        Vector3 velocity = playerRigidbody.velocity;
        playerRigidbody.velocity = new Vector3(velocity.x, 0f, velocity.z);
        playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        waitingForJumpLanding = true;
        jumpClearedGround = false;
        PlayFootSound(grassJump, woodJump);
    }

    private bool IsGrounded()
    {
        // Keep sleeping contacts, but discard support that was disabled or destroyed.
        groundContacts.RemoveWhere(IsInvalidGroundContact);
        return groundContacts.Count > 0;
    }

    private static bool IsInvalidGroundContact(Collider contact)
    {
        return contact == null || !contact.enabled || !contact.gameObject.activeInHierarchy;
    }

    private void OnCollisionEnter(Collision collision)
    {
        UpdateGroundContact(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        UpdateGroundContact(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        groundContacts.Remove(collision.collider);
    }

    private void UpdateGroundContact(Collision collision)
    {
        if (!isActiveAndEnabled)
        {
            return;
        }

        // Use the capsule's real support across its full footprint, including slopes and edges.
        // Trigger pickups do not generate collision contacts, and steep walls fail the normal check.
        for (int i = 0; i < collision.contactCount; i++)
        {
            ContactPoint contact = collision.GetContact(i);
            if (contact.thisCollider == playerCollider && contact.normal.y >= MinimumGroundNormalY)
            {
                groundContacts.Add(collision.collider);
                return;
            }
        }

        groundContacts.Remove(collision.collider);
    }

    private void OnDisable()
    {
        // Door travel can disable movement and teleport the player away from the old floor.
        groundContacts.Clear();
        stepTimer = 0f;
        waitingForJumpLanding = false;
        jumpClearedGround = false;
        jumpRequested = false;
        jumpBufferExpiresAt = float.NegativeInfinity;
        lastGroundedTime = float.NegativeInfinity;
        waitingForGroundSeparation = false;
    }

    private void OnDestroy()
    {
        Destroy(movementMaterial);
    }

    private void Update()
    {
        // Accumulate look input so physics steps neither lose nor repeat rendered-frame turns.
        pendingYawDegrees += yawInput * Time.deltaTime;
        UpdateCameraPitch();
    }

    private void FixedUpdate()
    {
        ApplyHorizontalDrag();
        ApplyMovement();
        ApplyBodyRotation();
        UpdateFootstepAudio();
        ApplyJump();
    }

    private void UpdateFootstepAudio()
    {
        if (footstepSound == null)
        {
            return;
        }

        bool grounded = IsGrounded();
        if (waitingForJumpLanding)
        {
            if (!grounded)
            {
                jumpClearedGround = true;
            }
            else if (jumpClearedGround)
            {
                PlayFootSound(grassLand, woodLand);
                waitingForJumpLanding = false;
                jumpClearedGround = false;
                stepTimer = 0.35f;
                return;
            }
            else if (!waitingForGroundSeparation)
            {
                // A blocked jump may never leave the floor.
                waitingForJumpLanding = false;
            }
        }

        Vector3 velocity = playerRigidbody.velocity;
        float groundSpeed = new Vector2(velocity.x, velocity.z).magnitude;
        // Require both input and ground speed so pushing into a wall stays quiet.
        if (!grounded || waitingForGroundSeparation ||
            (crouchSlide != null && crouchSlide.IsSliding) ||
            movementForce.sqrMagnitude < 0.01f || groundSpeed < 0.5f)
        {
            stepTimer = 0f;
            return;
        }

        stepTimer -= Time.fixedDeltaTime;
        if (stepTimer > 0f)
        {
            return;
        }

        PlayFootSound(grassWalk, woodWalk);
        stepTimer = 0.45f;
    }

    private void PlayFootSound(AudioClip grass, AudioClip wood)
    {
        DockHouseTravel travel = GetComponent<DockHouseTravel>();
        AudioClip step = travel != null && travel.IsInside ? wood : grass;
        if (footstepSound != null && step != null)
        {
            footstepSound.clip = step;
            footstepSound.Play();
        }
    }

    private void ApplyHorizontalDrag()
    {
        Vector3 velocity = playerRigidbody.velocity;
        float damping = Mathf.Clamp01(1f - horizontalDrag * Time.fixedDeltaTime);
        Vector3 horizontalVelocity = new Vector3(velocity.x, 0f, velocity.z) * damping;

        // Replace the missing floor friction with quick braking when movement is released.
        // Braking only on the ground preserves jump momentum and never slows the fall.
        if (movementForce.sqrMagnitude < 0.01f && IsGrounded())
        {
            horizontalVelocity = Vector3.MoveTowards(
                horizontalVelocity, Vector3.zero, groundBrakingDeceleration * Time.fixedDeltaTime);
        }

        playerRigidbody.velocity = new Vector3(horizontalVelocity.x, velocity.y, horizontalVelocity.z);
    }

    private void ApplyMovement()
    {
        // Local axes make forward input follow the player's facing direction after turning.
        playerRigidbody.AddRelativeForce(
            movementForce.x * Time.fixedDeltaTime,
            0,
            movementForce.y * Time.fixedDeltaTime);
    }

    private void ApplyBodyRotation()
    {
        // Explicit rotation still turns the body and its camera when physics rotation is frozen.
        Quaternion yawRotation = Quaternion.Euler(0, pendingYawDegrees, 0);
        playerRigidbody.MoveRotation(playerRigidbody.rotation * yawRotation);
        pendingYawDegrees = 0f;
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

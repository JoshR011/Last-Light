/*
 * Authors: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song
 * Last updated: 2026-11-06
 * Summary: Pressing crouch toggles between crouching and standing; crouching shrinks the
 *          player's collider and lowers the camera. Pressing slide on the ground starts a
 *          slide that loses speed over time, then returns the player to the state they
 *          were in before the slide. The headroomLayers was from AI since when uncrouching under
 *          a low ceiling the player was cliping and I was not sure how to fix it.
 */

using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerCrouchSlide : MonoBehaviour
{
    [Header("References")]
    public Transform cameraTransform;

    [Header("Crouching")]
    public float crouchMaxSpeed = 3f;
    public float cameraTransitionSpeed = 10f;
    // Layers that can block standing up. Leave the player's own layer unticked.
    public LayerMask headroomLayers = ~0;

    [Header("Sliding")]
    public float slideSpeed = 12f;
    // Speed lost per second while sliding.
    public float slideDeceleration = 10f;
    // The slide ends at this speed; set it to the player's normal walking speed.
    public float slideEndSpeed = 5f;
    // Seconds after a slide ends before another slide can start.
    public float slideCooldownSeconds = 1f;

    private const float GroundCheckMargin = 0.1f;
    private const float MinimumGroundNormalY = 0.5f;
    private const float HeadroomRadiusScale = 0.95f;

    private Rigidbody playerRigidbody;
    private CapsuleCollider playerCollider;

    private float standingHeight;
    private float crouchHeight;
    private Vector3 standingCenter;
    private Vector3 standingCameraPosition;

    // What the player chose with the crouch key: true = crouch, false = stand.
    private bool crouchToggled;
    // Whether the collider is actually in its crouched shape right now.
    private bool isCrouching;
    private bool slideRequested;
    private bool isSliding;
    private Vector3 slideDirection;
    private float nextSlideTime;
    private Vector2 movementInput;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();

        // Remember the standing shape so crouching can be undone exactly.
        standingHeight = playerCollider.height;
        standingCenter = playerCollider.center;

        // Crouch to half the standing height.
        crouchHeight = standingHeight / 2f;
        standingCameraPosition = cameraTransform.localPosition;
    }

    public void OnMove(InputValue value)
    {
        // PlayerInput sends Move to every script on this GameObject, so the slide can tell whether a direction is held.
        movementInput = value.Get<Vector2>();
    }

    public void OnCrouch(InputValue value)
    {
        // Each press flips between crouching and standing.
        if (value.isPressed)
        {
            crouchToggled = !crouchToggled;
        }
    }

    public void OnSlide(InputValue value)
    {
        // Retain a press until the next physics step, which checks that the player is on the ground.
        if (value.isPressed)
        {
            slideRequested = true;
        }
    }

    private void Update()
    {
        // A slide keeps the player low; afterwards crouchToggled still holds the pre-slide choice.
        if ((crouchToggled || isSliding) && !isCrouching)
        {
            SetCrouching(true);
        }
        // Stay crouched under low ceilings; the player stands up automatically once there is room.
        else if (!(crouchToggled || isSliding) && isCrouching && HasHeadroom())
        {
            SetCrouching(false);
        }

        UpdateCameraHeight();
    }

    private void FixedUpdate()
    {
        if (slideRequested)
        {
            slideRequested = false;
            TryStartSlide();
        }

        if (isSliding)
        {
            ApplySlide();
        }
        else if (isCrouching)
        {
            LimitCrouchSpeed();
        }
    }

    private void TryStartSlide()
    {
        // Ignore presses during a slide so it cannot be restarted for a fresh boost.
        if (isSliding)
        {
            return;
        }

        // When crouched, the slide key requests standing instead of starting a slide.
        // Update keeps the player low until there is enough headroom.
        if (crouchToggled || isCrouching)
        {
            crouchToggled = false;
            return;
        }

        if (!IsGrounded())
        {
            return;
        }

        // With no direction held, the slide key just crouches.
        if (movementInput.sqrMagnitude < 0.01f)
        {
            crouchToggled = true;
            return;
        }

        // Ignore presses until the cooldown from the last slide has passed.
        if (Time.time < nextSlideTime)
        {
            return;
        }

        Vector3 horizontalVelocity = GetHorizontalVelocity();

        isSliding = true;
        // Lower the collider right away so the slide fits under obstacles from its first step.
        SetCrouching(true);

        // Slide the way the player is moving, or the way they are pressing if not moving yet.
        if (horizontalVelocity.sqrMagnitude < 0.01f)
        {
            slideDirection = transform.TransformDirection(new Vector3(movementInput.x, 0f, movementInput.y)).normalized;
        }
        else
        {
            slideDirection = horizontalVelocity.normalized;
        }

        // Never slow the player down when the slide starts; extra speed makes the slide last longer.
        float startSpeed = Mathf.Max(horizontalVelocity.magnitude, slideSpeed);
        SetHorizontalVelocity(slideDirection * startSpeed);
    }

    private void ApplySlide()
    {
        // Lose a little speed every physics step; physics friction and slopes also affect the measured speed.
        float currentSpeed = GetHorizontalVelocity().magnitude - slideDeceleration * Time.fixedDeltaTime;

        if (currentSpeed <= slideEndSpeed)
        {
            // Finish at walking speed; Update then returns the player to their pre-slide crouch state.
            SetHorizontalVelocity(slideDirection * slideEndSpeed);
            isSliding = false;
            // Start the cooldown when the slide ends so slides cannot be chained back to back.
            nextSlideTime = Time.time + slideCooldownSeconds;
            return;
        }

        SetHorizontalVelocity(slideDirection * currentSpeed);
    }

    private void LimitCrouchSpeed()
    {
        Vector3 horizontalVelocity = GetHorizontalVelocity();

        if (horizontalVelocity.magnitude > crouchMaxSpeed)
        {
            // Cap only horizontal speed so falling and jumping are unaffected.
            SetHorizontalVelocity(horizontalVelocity.normalized * crouchMaxSpeed);
        }
    }

    private void SetCrouching(bool crouch)
    {
        isCrouching = crouch;

        if (crouch)
        {
            // Shrink from the top and move the center down so the feet stay on the ground.
            float heightLost = standingHeight - crouchHeight;
            playerCollider.height = crouchHeight;
            playerCollider.center = standingCenter - Vector3.up * (heightLost / 2f);
        }
        else
        {
            playerCollider.height = standingHeight;
            playerCollider.center = standingCenter;
        }
    }

    private bool HasHeadroom()
    {
        // Cast upward from the top of the crouched capsule by the height the player would gain.
        float radius = playerCollider.radius * HeadroomRadiusScale;
        Vector3 topSphereCenter = transform.TransformPoint(
            playerCollider.center + Vector3.up * (playerCollider.height / 2f - playerCollider.radius));
        float distance = standingHeight - crouchHeight;

        return !Physics.SphereCast(
            topSphereCenter, radius, Vector3.up, out _,
            distance, headroomLayers, QueryTriggerInteraction.Ignore);
    }

    private void UpdateCameraHeight()
    {
        // Lower the camera by the same amount the collider shrinks, smoothly over a few frames.
        Vector3 targetPosition = isCrouching
            ? standingCameraPosition - Vector3.up * (standingHeight - crouchHeight)
            : standingCameraPosition;

        cameraTransform.localPosition = Vector3.Lerp(
            cameraTransform.localPosition, targetPosition, cameraTransitionSpeed * Time.deltaTime);
    }

    private Vector3 GetHorizontalVelocity()
    {
        Vector3 velocity = playerRigidbody.velocity;
        return new Vector3(velocity.x, 0f, velocity.z);
    }

    private void SetHorizontalVelocity(Vector3 horizontalVelocity)
    {
        // Replace ground speed but keep vertical speed so gravity and jumps still work.
        playerRigidbody.velocity = new Vector3(horizontalVelocity.x, playerRigidbody.velocity.y, horizontalVelocity.z);
    }

    private bool IsGrounded()
    {
        // Same check as PlayerMovement: ignore triggers and steep walls.
        Bounds bounds = playerCollider.bounds;

        return Physics.Raycast(
            bounds.center, Vector3.down, out RaycastHit hit,
            bounds.extents.y + GroundCheckMargin, Physics.DefaultRaycastLayers,
            QueryTriggerInteraction.Ignore) && hit.normal.y >= MinimumGroundNormalY;
    }
}
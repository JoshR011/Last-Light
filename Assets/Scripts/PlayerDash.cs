/*
 * Authors: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song
 * Last updated: 2026-10-21
 * Summary: Launches the player in their movement direction when the dash input is pressed,
 *          with a cooldown between dashes. momentum is preserved before dash and after.
 */

using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerDash : MonoBehaviour
{
    [Header("Dashing")]
    public float dashImpulse = 15f;
    public float dashCooldownSeconds = 1f;

    private Vector2 movementInput;
    private bool dashRequested;
    private float nextDashTime;
    private Rigidbody playerRigidbody;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    public void OnMove(InputValue value)
    {
        
        movementInput = value.Get<Vector2>();
    }

    public void OnDash(InputValue value)
    {
        // Retain a press until the next physics step, ignoring button releases.
        if (value.isPressed)
        {
            dashRequested = true;
        }
    }

    private void FixedUpdate()
    {
        ApplyDash();
    }

    private void ApplyDash()
    {
        if (!dashRequested)
        {
            return;
        }

        dashRequested = false;

        // Ignore presses until the cooldown has passed.
        if (Time.time < nextDashTime)
        {
            return;
        }

        // Dash in the direction the player is pressing, or straight forward if no direction is held.
        Vector3 localDirection = new Vector3(movementInput.x, 0, movementInput.y);
        if (localDirection.sqrMagnitude < 0.01f)
        {
            localDirection = Vector3.forward;
        }
        Vector3 dashDirection = transform.TransformDirection(localDirection.normalized);


        playerRigidbody.AddForce(dashDirection * dashImpulse, ForceMode.Impulse);
        nextDashTime = Time.time + dashCooldownSeconds;
    }
}
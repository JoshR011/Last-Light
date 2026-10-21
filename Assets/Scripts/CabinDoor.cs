/*
 * Authors: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song
 * Last updated: 2026-10-21
 * Summary: Swings the cabin door open when the keypad grants access.
 */

using UnityEngine;

public class CabinDoor : MonoBehaviour
{
    [SerializeField] private float openAngle = 90f;
    [SerializeField, Min(1f)] private float openingSpeed = 90f;

    private Quaternion openRotation;
    private bool isOpening;
    private bool isOpen;

    private void Awake()
    {
        // Use the starting local rotation so the hinge motion follows the door's scene orientation.
        openRotation = transform.localRotation * Quaternion.Euler(0f, openAngle, 0f);
    }

    public void OpenDoor()
    {
        // Repeated access-granted events must not restart or close the door.
        if (isOpen) return;

        isOpen = true;
        isOpening = true;
    }

    private void Update()
    {
        if (!isOpening) return;

        // Rotate the mesh and collider together at a frame-rate-independent speed.
        transform.localRotation = Quaternion.RotateTowards(
            transform.localRotation, openRotation, openingSpeed * Time.deltaTime);

        // Snap to the final angle and stop updating once the door is fully open.
        if (Quaternion.Angle(transform.localRotation, openRotation) < 0.01f)
        {
            transform.localRotation = openRotation;
            isOpening = false;
        }
    }
}

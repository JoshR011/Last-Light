/*
 * Authors: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song
 * Date Created: 2026-11-20
 * Date Last Updated: 2026-11-20
 * Summary: Recharges the player flashlight and consumes the pickup when a charge is needed.
 */

using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider))]
public sealed class BatteryPickup : MonoBehaviour
{
    private bool collected;

    private void OnTriggerEnter(Collider other)
    {
        // Several child colliders may enter before Unity destroys the pickup.
        if (collected) return;

        // Resolve the player root even if a collider lower in the hierarchy touched the trigger.
        Transform player = other.attachedRigidbody != null ? other.attachedRigidbody.transform : other.transform.root;
        if (!player.CompareTag("Player")) return;

        FlashlightController flashlight = player.GetComponentInChildren<FlashlightController>();
        if (flashlight == null || !flashlight.TryRecharge()) return;

        collected = true;
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}

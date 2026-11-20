using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider))]
public sealed class BatteryPickup : MonoBehaviour
{
    private bool collected;

    private void OnTriggerEnter(Collider other)
    {
        if (collected) return;

        Transform player = other.attachedRigidbody != null ? other.attachedRigidbody.transform : other.transform.root;
        if (!player.CompareTag("Player")) return;

        FlashlightController flashlight = player.GetComponentInChildren<FlashlightController>();
        if (flashlight == null || !flashlight.TryRecharge()) return;

        collected = true;
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}

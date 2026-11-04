/*
 * Authors: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song
 * Last updated: 2026-11-03
 * Summary: Lets the player collect a ground weapon by walking into its trigger.
 */

using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WeaponPickup : MonoBehaviour
{
    [Header("Weapon Pickup")]
    public WeaponType weaponType;

    private bool collected;

    private void OnTriggerEnter(Collider other)
    {
        // Several player colliders can enter before the pickup is destroyed at the end of the frame.
        if (collected)
        {
            return;
        }

        // Resolve the player that touched this pickup instead of using a global inventory.
        Transform player = other.attachedRigidbody != null ? other.attachedRigidbody.transform : other.transform.root;
        // Projectiles and enemies can touch the trigger, but only the player can collect its weapon.
        if (!player.CompareTag("Player"))
        {
            return;
        }

        InventoryManager inventory = player.GetComponentInChildren<InventoryManager>(true);
        // Leave the pickup on the ground if this player has no inventory or cannot equip this weapon type.
        if (inventory == null || !inventory.TryCollectWeapon(weaponType))
        {
            return;
        }

        collected = true;
        // Hide immediately; several player colliders can enter before Destroy completes.
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}

/*
 * Authors: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song
 * Date Created: 2026-11-03
 * Date Last Updated: 2026-11-20
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
        // Leave the pickup in the world if the player cannot store it.
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

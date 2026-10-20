/*
 * Authors: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song
 * Last updated: 2026-10-19
 * Summary: Consumes a projectile on contact and damages objects with health.
 */

using UnityEngine;
using UnityEngine.Serialization;

public class ContactDamager : MonoBehaviour
{
    [Header("Contact Damage")]
    // Preserve damage values already saved in projectile prefabs under the old name.
    [FormerlySerializedAs("damage")]
    public float damageAmount;

    private void OnTriggerEnter(Collider other)
    {
        // Projectiles also disappear against scenery that has no Life component.
        Destroy(gameObject);

        Life targetLife = other.GetComponent<Life>();

        // Scenery can stop a projectile without having health to subtract from.
        if (targetLife != null)
        {
            targetLife.currentHealth -= damageAmount;
        }
    }
}

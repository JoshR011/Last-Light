/*
 * Authors: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song
 * Last updated: 2026-11-04
 * Summary: Consumes a projectile on contact and reports surface hits to objects with health.
 */

using UnityEngine;
using UnityEngine.Serialization;

public class ContactDamager : MonoBehaviour
{
    [Header("Contact Damage")]
    // Preserve damage values already saved in projectile prefabs under the old name.
    [FormerlySerializedAs("damage")]
    public float damageAmount;

    private bool contactHandled;

    private void OnTriggerEnter(Collider other)
    {
        // Compound enemy colliders can report several contacts before deferred destruction finishes.
        if (contactHandled) return;
        contactHandled = true;

        // Projectiles also disappear against scenery that has no Life component.
        Destroy(gameObject);

        Life targetLife = other.GetComponentInParent<Life>();

        // Scenery can stop a projectile without having health to subtract from.
        if (targetLife != null)
        {
            Rigidbody body = GetComponent<Rigidbody>();
            Vector3 direction = body != null && body.velocity.sqrMagnitude > 0.0001f
                ? body.velocity.normalized : transform.forward;

            // Trigger callbacks have no contact points, so trace the struck collider back to its entry surface.
            float backtrackDistance = other.bounds.size.magnitude + 0.1f;
            Ray entryRay = new Ray(transform.position - direction * backtrackDistance, direction);
            Vector3 hitPoint = other.ClosestPoint(transform.position);
            Vector3 hitNormal = -direction;
            if (other.Raycast(entryRay, out RaycastHit hit, backtrackDistance + 0.1f))
            {
                hitPoint = hit.point;
                hitNormal = hit.normal;
            }

            targetLife.TakeProjectileDamage(damageAmount, hitPoint, hitNormal);
        }
    }
}

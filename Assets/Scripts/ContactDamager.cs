/*
 * Authors: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song
 * Last updated: 2026-11-04
 * Summary: Sweeps a projectile's travelled path and reports its first surface hit to objects with health.
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
    private Rigidbody projectileBody;
    private Collider projectileCollider;
    private SphereCollider projectileSphere;
    private Vector3 previousPosition;
    private readonly RaycastHit[] sweepHits = new RaycastHit[8];

    private void Awake()
    {
        projectileBody = GetComponent<Rigidbody>();
        projectileCollider = GetComponent<Collider>();
        projectileSphere = GetComponent<SphereCollider>();
        previousPosition = GetProjectileCenter();
    }

    private void FixedUpdate()
    {
        // Fast trigger bullets can cross a whole target between physics updates without overlapping it.
        if (contactHandled) return;
        CheckTravelledPath();
        previousPosition = GetProjectileCenter();
    }

    private Vector3 GetProjectileCenter()
    {
        // Rigidbody positions follow physics even when interpolation smooths the visible Transform.
        Vector3 position = projectileBody != null ? projectileBody.position : transform.position;
        if (projectileSphere == null) return position;
        Quaternion rotation = projectileBody != null ? projectileBody.rotation : transform.rotation;
        return position + rotation * Vector3.Scale(projectileSphere.center, transform.lossyScale);
    }

    private bool CanHit(Collider other)
    {
        // Pickup and sensor triggers must not consume bullets before they reach a solid target.
        if (other == null || other.isTrigger || other.transform.IsChildOf(transform)) return false;
        if (projectileBody != null && other.attachedRigidbody == projectileBody) return false;
        if (Physics.GetIgnoreLayerCollision(gameObject.layer, other.gameObject.layer)) return false;
        return projectileCollider == null || !Physics.GetIgnoreCollision(projectileCollider, other);
    }

    private bool CheckTravelledPath()
    {
        Vector3 travel = GetProjectileCenter() - previousPosition;
        float distance = travel.magnitude;
        if (distance < 0.00001f) return false;
        Vector3 direction = travel / distance;
        Vector3 scale = transform.lossyScale;
        float radius = projectileSphere != null
            ? projectileSphere.radius * Mathf.Max(Mathf.Abs(scale.x), Mathf.Abs(scale.y), Mathf.Abs(scale.z))
            : 0.001f;

        int count = Physics.SphereCastNonAlloc(previousPosition, radius, direction, sweepHits,
            distance, Physics.AllLayers, QueryTriggerInteraction.Ignore);
        RaycastHit[] hits = sweepHits;
        // A full buffer can omit the closest collider, so query every hit in unusually crowded paths.
        if (count == sweepHits.Length)
        {
            hits = Physics.SphereCastAll(previousPosition, radius, direction, distance,
                Physics.AllLayers, QueryTriggerInteraction.Ignore);
            count = hits.Length;
        }

        int closest = -1;
        for (int i = 0; i < count; i++)
        {
            if (CanHit(hits[i].collider) && (closest < 0 || hits[i].distance < hits[closest].distance))
            {
                closest = i;
            }
        }
        if (closest < 0) return false;

        RaycastHit hit = hits[closest];
        HandleContact(hit.collider, hit.point, hit.normal);
        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (contactHandled || !CanHit(other)) return;
        // Resolve earlier surfaces first when a step crosses a wall and ends inside another object.
        if (CheckTravelledPath()) return;

        Vector3 direction = projectileBody != null && projectileBody.velocity.sqrMagnitude > 0.0001f
            ? projectileBody.velocity.normalized : transform.forward;

        // Initial overlaps have no swept entry, so recover a surface point from the struck collider.
        float backtrackDistance = other.bounds.size.magnitude + 0.1f;
        Vector3 position = GetProjectileCenter();
        Ray entryRay = new Ray(position - direction * backtrackDistance, direction);
        Vector3 hitPoint = other.ClosestPoint(position);
        Vector3 hitNormal = -direction;
        if (other.Raycast(entryRay, out RaycastHit hit, backtrackDistance + 0.1f))
        {
            hitPoint = hit.point;
            hitNormal = hit.normal;
        }
        HandleContact(other, hitPoint, hitNormal);
    }

    private void HandleContact(Collider other, Vector3 hitPoint, Vector3 hitNormal)
    {
        // Sweep checks and compound colliders can report the same projectile before destruction finishes.
        if (contactHandled) return;
        contactHandled = true;
        Destroy(gameObject);

        Life targetLife = other.GetComponentInParent<Life>();
        if (targetLife != null) targetLife.TakeProjectileDamage(damageAmount, hitPoint, hitNormal);
    }
}

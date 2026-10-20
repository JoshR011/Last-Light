/*
 * Authors: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song
 * Last updated: 2026-10-19
 * Summary: Finds the first unobstructed target inside the sensor's viewing cone.
 */

using UnityEngine;
using UnityEngine.Serialization;

public class Sight : MonoBehaviour
{
    [Header("Viewing Cone")]
    [FormerlySerializedAs("distance")]
    public float viewDistance;
    // The angle test measures from forward to one edge, so this is half the full viewing cone.
    [FormerlySerializedAs("angle")]
    public float viewHalfAngle;

    [Header("Detection Layers")]
    // Separate filters let scenery block sight without becoming a target itself.
    [FormerlySerializedAs("objectsLayers")]
    public LayerMask targetLayers;
    [FormerlySerializedAs("obstaclesLayers")]
    public LayerMask obstacleLayers;

    [Header("Detection Result")]
    [FormerlySerializedAs("detectedObject")]
    public Collider detectedTarget;

    private void Update()
    {
        // Replacing the result with null when nothing is visible prevents chasing a stale detection.
        detectedTarget = FindVisibleTarget();
    }

    private Collider FindVisibleTarget()
    {
        // Filter by range and target layers before spending work on angles and line-of-sight tests.
        Collider[] candidates = Physics.OverlapSphere(transform.position, viewDistance, targetLayers);

        foreach (Collider candidate in candidates)
        {
            if (IsTargetVisible(candidate))
            {
                // The FSM needs only one visible target, so further visibility checks are unnecessary.
                return candidate;
            }
        }

        return null;
    }

    private bool IsTargetVisible(Collider candidate)
    {
        // Test the collider center because a character's transform can be at its feet.
        Vector3 targetCenter = candidate.bounds.center;
        Vector3 directionToTarget = (targetCenter - transform.position).normalized;
        float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

        // Short-circuiting avoids an obstacle query for targets outside the viewing cone.
        return angleToTarget < viewHalfAngle
            && !Physics.Linecast(transform.position, targetCenter, obstacleLayers);
    }
}

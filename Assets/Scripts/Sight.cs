// Team members: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Detects the first nearby target inside the viewing cone that is not hidden by obstacles.
public class Sight : MonoBehaviour
{
    // Configure viewing range, maximum angle from forward, and target/obstacle layer filters.
    public float distance;
    public float angle;
    public LayerMask objectsLayers;
    public LayerMask obstaclesLayers;

    // The visible target for this frame, or null when none passes the sight checks.
    public Collider detectedObject;

    private void Update()
    {
        // Find colliders on the target layers within viewing distance.
        Collider[] colliders = Physics.OverlapSphere(
            transform.position,
            distance,
            objectsLayers
        );

        // Clear the previous frame's detection.
        detectedObject = null;

        foreach (Collider candidate in colliders)
        {
            // Direction from the enemy's eyes to the target's center.
            Vector3 direction = (
                candidate.bounds.center - transform.position
            ).normalized;

            // Measure the target's angle from forward before checking the viewing cone.
            float angleToTarget = Vector3.Angle(
                transform.forward,
                direction
            );

            if (angleToTarget < angle)
            {
                // Check whether anything on the obstacle layers blocks the target's center.
                bool blocked = Physics.Linecast(
                    transform.position,
                    candidate.bounds.center,
                    obstaclesLayers
                );

                // Keep the first unobstructed target and stop checking the remaining candidates.
                if (!blocked)
                {
                    detectedObject = candidate;
                    break;
                }
            }
        }
    }
}

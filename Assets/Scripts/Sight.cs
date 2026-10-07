using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : MonoBehaviour
{
    public float distance;
    public float angle;
    public LayerMask objectsLayers;
    public LayerMask obstaclesLayers;

    public Collider detectedObject;

    private void Update()
    {
        // 1. Find potential targets within viewing distance.
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

            // 2. Check whether the target is inside our viewing cone.
            float angleToTarget = Vector3.Angle(
                transform.forward,
                direction
            );

            if (angleToTarget < angle)
            {
                // 3. Check whether an obstacle blocks our view.
                bool blocked = Physics.Linecast(
                    transform.position,
                    candidate.bounds.center,
                    obstaclesLayers
                );

                if (!blocked)
                {
                    detectedObject = candidate;
                    break;
                }
            }
        }
    }
}

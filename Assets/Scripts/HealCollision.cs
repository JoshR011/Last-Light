using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Lets a player below full health consume this pickup to restore health.
public class HealCollision : MonoBehaviour
{
    // Healing value used by the pickup's health-increase calculation.
    public float heal;
    void OnTriggerEnter(Collider other)
    {
        // Only colliders tagged as the player can activate this health pickup.
        if (other.CompareTag("Player"))
        {
            Life life = other.GetComponent<Life>();

            // Consume the pickup when the player's health is below the full-health value of 100.
            if (life.amount < 100)
            {
                Destroy(gameObject);

                // Calculate the health increase using the heal value and remaining health gap.
                if (life != null)
                {
                    float amt = (life.amount <= life.amount - heal) ? heal : (100 - life.amount);
                    life.amount += amt;
                }
            }
        }
    }
}

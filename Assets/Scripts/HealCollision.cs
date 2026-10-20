// Team members: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song

using UnityEngine;

// Lets a player below full health consume this pickup to restore health.
public class HealCollision : MonoBehaviour
{
    // Amount restored by this pickup, capped at full health.
    public float heal;
    void OnTriggerEnter(Collider other)
    {
        // Only colliders tagged as the player can activate this health pickup.
        if (other.CompareTag("Player"))
        {
            Life life = other.GetComponent<Life>();

            // Only consume the pickup when it can restore health.
            if (life != null && life.amount < 100 && heal > 0)
            {
                life.amount += Mathf.Min(heal, 100f - life.amount);
                Destroy(gameObject);
            }
        }
    }
}

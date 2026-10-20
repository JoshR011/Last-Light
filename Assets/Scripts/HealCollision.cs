/*
 * Authors: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song
 * Last updated: 2026-10-19
 * Summary: Lets an injured player consume a pickup to restore health, capped at 100.
 */

using UnityEngine;
using UnityEngine.Serialization;

public class HealCollision : MonoBehaviour
{
    private const float FullHealth = 100f;

    [Header("Healing")]
    [FormerlySerializedAs("heal")]
    public float healAmount;

    private void OnTriggerEnter(Collider other)
    {
        // Enemies can also have Life components, but these pickups are reserved for the player.
        if (!other.CompareTag("Player"))
        {
            return;
        }

        Life playerLife = other.GetComponent<Life>();

        // Leave the pickup available when this contact cannot restore health.
        if (playerLife == null || playerLife.currentHealth >= FullHealth || healAmount <= 0)
        {
            return;
        }

        // Near full health, restore only the remaining gap to prevent overhealing.
        playerLife.currentHealth += Mathf.Min(healAmount, FullHealth - playerLife.currentHealth);
        // Consume the pickup only after a valid heal so unsuccessful contacts do not waste it.
        Destroy(gameObject);
    }
}

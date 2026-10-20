/*
 * Authors: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song
 * Last updated: 2026-10-19
 * Summary: Stores health and invokes the configured response while depleted.
 */

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Life : MonoBehaviour
{
    [Header("Health")]
    // Existing scenes and prefabs still store their starting health under the old field name.
    [FormerlySerializedAs("amount")]
    public float currentHealth;

    [Header("Depletion Response")]
    // Inspector listeners let crates and enemies share health while choosing different break responses.
    public UnityEvent onBreak;

    private void Update()
    {
        // Include negative health because a final hit can deal more damage than the health remaining.
        if (currentHealth <= 0)
        {
            onBreak.Invoke();
        }
    }

    // Keep this public so Inspector events can use it as the depletion response.
    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}

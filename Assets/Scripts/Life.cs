/*
 * Authors: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song
 * Last updated: 2026-11-06
 * Summary: Stores health, reports damage, and invokes the configured depletion response once.
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
    public UnityEvent onBreak = new UnityEvent();

    private bool depletionHandled;

    // Feedback can react to a hit immediately without changing the object's depletion response.
    public event System.Action Damaged;

    // Projectile feedback receives the surface hit while other damage sources keep using TakeDamage.
    public event System.Action<Vector3, Vector3> ProjectileHit;

    public void TakeProjectileDamage(float damageAmount, Vector3 hitPoint, Vector3 hitNormal)
    {
        // Rejected damage must not produce blood or other hit feedback.
        if (damageAmount <= 0f || currentHealth <= 0f) return;
        TakeDamage(damageAmount);
        ProjectileHit?.Invoke(hitPoint, hitNormal);
    }

    public void TakeDamage(float damageAmount)
    {
        // Healing, zero damage, and hits on depleted objects should not trigger a damage flash.
        if (damageAmount <= 0f || currentHealth <= 0f) return;
        currentHealth = Mathf.Max(0f, currentHealth - damageAmount);
        Damaged?.Invoke();
    }

    private void Update()
    {
        // Inspector edits can also deplete health; run the response once even if destruction is deferred.
        if (currentHealth <= 0 && !depletionHandled)
        {
            depletionHandled = true;
            currentHealth = 0f;
            onBreak?.Invoke();
        }
    }

    // The player's depletion event uses this response instead of the crate or enemy's break behavior.
    public void PlayerDied()
    {
        Debug.Log("you died", this);
        DestroyObject();
    }

    // Keep this public so Inspector events can use it as the depletion response.
    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}

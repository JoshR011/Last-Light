/*
 * Authors: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song
 * Last updated: 2026-11-04
 * Summary: Sprays blood at bullet impacts and replaces a dead zombie with a burst of chunks.
 */

using UnityEngine;

[RequireComponent(typeof(Life))]
public class ZombieDamageEffects : MonoBehaviour
{
    [Header("Damage Effects")]
    public GameObject bloodSprayPrefab;
    public GameObject deathChunksPrefab;

    [Header("Effect Cleanup")]
    [Min(0.1f)]
    public float effectLifetimeSeconds = 4f;

    private Life zombieLife;
    private bool deathEffectPlayed;

    private void Awake()
    {
        zombieLife = GetComponent<Life>();
    }

    private void OnEnable()
    {
        // Listen for bullets only; other damage can still kill the zombie without spraying impact blood.
        zombieLife.ProjectileHit += SprayBlood;
    }

    private void SprayBlood(Vector3 hitPoint, Vector3 hitNormal)
    {
        Vector3 outward = hitNormal.sqrMagnitude > 0.0001f ? hitNormal.normalized : -transform.forward;
        // A slight surface offset keeps the spray visible instead of clipping it inside the zombie.
        SpawnEffect(bloodSprayPrefab, hitPoint + outward * 0.01f, Quaternion.LookRotation(outward));
    }

    // Life's depletion event invokes this response once, including deaths caused by non-projectile damage.
    public void Explode()
    {
        if (deathEffectPlayed) return;
        deathEffectPlayed = true;

        // Use the visible body's center so the chunk burst starts above the feet on every zombie instance.
        Renderer[] bodyRenderers = GetComponentsInChildren<Renderer>();
        Vector3 center = transform.position + Vector3.up;
        if (bodyRenderers.Length > 0)
        {
            Bounds bounds = bodyRenderers[0].bounds;
            foreach (Renderer bodyRenderer in bodyRenderers)
            {
                bounds.Encapsulate(bodyRenderer.bounds);
            }
            center = bounds.center;
        }

        SpawnEffect(deathChunksPrefab, center, Quaternion.identity);
        Destroy(gameObject);
    }

    private void SpawnEffect(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (prefab == null) return;
        // Independent world effects survive the zombie's destruction and keep their authored orientation.
        GameObject effect = Instantiate(prefab, position, rotation * prefab.transform.localRotation);
        Destroy(effect, effectLifetimeSeconds);
    }

    private void OnDisable()
    {
        zombieLife.ProjectileHit -= SprayBlood;
    }
}

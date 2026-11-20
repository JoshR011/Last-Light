/*
 * Authors: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song
 * Last updated: 2026-11-04
 * Summary: Sprays blood at bullet impacts and replaces a dead zombie with a burst of chunks.
 */

using UnityEngine;

[RequireComponent(typeof(Life))]
public class ZombieDamageEffects : MonoBehaviour
{
    private static readonly int AttackState = Animator.StringToHash("Attack");
    private static readonly int HitTrigger = Animator.StringToHash("Hit");
    private static readonly int DeadParameter = Animator.StringToHash("Dead");
    private static readonly int DeathState = Animator.StringToHash("Base Layer.Death");

    [Header("Damage Effects")]
    public GameObject bloodSprayPrefab;
    public GameObject deathChunksPrefab;

    [Header("Effect Cleanup")]
    [Min(0.1f)]
    public float effectLifetimeSeconds = 4f;

    private Life zombieLife;
    private Animator animator;
    private bool deathStarted;

    private void Awake()
    {
        zombieLife = GetComponent<Life>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        zombieLife.ProjectileHit += SprayBlood;
        zombieLife.Damaged += PlayHit;
    }

    private void PlayHit()
    {
        if (zombieLife.currentHealth <= 0f || animator == null || !animator.isActiveAndEnabled ||
            animator.runtimeAnimatorController == null)
        {
            return;
        }

        // Let a swing in progress reach its impact pose while blood still shows the hit.
        if (animator.GetCurrentAnimatorStateInfo(0).shortNameHash != AttackState &&
            animator.GetNextAnimatorStateInfo(0).shortNameHash != AttackState)
        {
            animator.SetTrigger(HitTrigger);
        }
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
        if (deathStarted) return;
        deathStarted = true;

        if (animator == null || !animator.isActiveAndEnabled || animator.runtimeAnimatorController == null ||
            !animator.HasState(0, DeathState))
        {
            FinishDeath();
            return;
        }

        // Keep the falling body from blocking shots or movement during the death clip.
        if (TryGetComponent(out Collider bodyCollider))
        {
            bodyCollider.enabled = false;
        }
        animator.SetBool(DeadParameter, true);
    }

    private void LateUpdate()
    {
        if (!deathStarted) return;
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        if (state.fullPathHash == DeathState && state.normalizedTime >= 1f)
        {
            FinishDeath();
        }
    }

    private void FinishDeath()
    {
        enabled = false;
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
        zombieLife.Damaged -= PlayHit;
    }
}

/*
 * Authors: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song
 * Last updated: 2026-10-21
 * Summary: Plays a break explosion and reveals the winning crate or a temporary hint.
 */

using UnityEngine;

public class Crate : MonoBehaviour
{
    private const float HintLifetimeSeconds = 4f;

    [Header("Hint Prefab")]
    public GameObject hintLine;

    [Header("Break Effect")]
    public GameObject explosionEffect;
    [Min(0.01f)]
    public float explosionScale = 0.5f;
    [Min(0.1f)]
    public float explosionLifetime = 20f;

    private Life crateLife;
    private bool isBroken;

    private void Awake()
    {
        // Subscribing here lets any source of damage trigger the same crate response.
        crateLife = GetComponent<Life>();
        if (crateLife == null || crateLife.onBreak == null)
        {
            Debug.LogError("Crate requires a Life component with an onBreak event.", this);
            return;
        }

        crateLife.onBreak.AddListener(HandleCrateBroken);
    }

    private void HandleCrateBroken()
    {
        // Health events or multiple hits can arrive before Destroy runs at the end of the frame.
        if (isBroken)
        {
            return;
        }

        isBroken = true;
        SpawnExplosion();
        CrateManager manager = CrateManager.instance;
        if (manager == null)
        {
            Debug.LogWarning("Crate broke without an active CrateManager; no hint can be shown.", this);
        }
        else if (!manager.IsSolved && manager.winningCrate != null)
        {
            Crate target = manager.winningCrate;
            // The winning crate ends the search, so it does not need to reveal another hint.
            if (target == this)
            {
                manager.CompletePuzzle();
                print("You found the right crate!");
            }
            else
            {
                SpawnDirectionalHint(target);
            }
        }

        // Remove the broken crate so it cannot obstruct the player or produce more hints.
        Destroy(gameObject);
    }

    private void SpawnExplosion()
    {
        if (explosionEffect == null)
        {
            return;
        }

        Collider crateCollider = GetComponent<Collider>();
        Vector3 position = crateCollider != null ? crateCollider.bounds.center : transform.position;
        // Keep the prefab's authored orientation and leave it unparented so it survives the crate.
        GameObject explosion = Instantiate(explosionEffect, position, explosionEffect.transform.rotation);
        explosion.transform.localScale *= explosionScale;
        // Simple FX's rubble lives for 15 seconds; allow the entire effect to finish before cleanup.
        Destroy(explosion, explosionLifetime);
    }

    private void SpawnDirectionalHint(Crate target)
    {
        if (hintLine == null)
        {
            Debug.LogWarning("Crate has no hint prefab assigned.", this);
            return;
        }

        // Collider centers keep the hint aligned even when a model's pivot is offset.
        Collider sourceCollider = GetComponent<Collider>();
        Collider targetCollider = target.GetComponent<Collider>();
        Vector3 hintPosition = sourceCollider != null ? sourceCollider.bounds.center : transform.position;
        Vector3 targetPosition = targetCollider != null ? targetCollider.bounds.center : target.transform.position;
        Vector3 hintDirection = targetPosition - hintPosition;

        if (hintDirection.sqrMagnitude < 0.0001f)
        {
            return;
        }

        // Align the hint's forward axis with the target instead of inheriting the crate's rotation.
        GameObject hint = Instantiate(hintLine, hintPosition, Quaternion.identity);
        hint.transform.rotation = Quaternion.LookRotation(hintDirection, hint.transform.up);
        // A short lifetime gives the player a clue without leaving permanent scene clutter.
        Destroy(hint, HintLifetimeSeconds);
    }

    private void OnDestroy()
    {
        if (crateLife != null && crateLife.onBreak != null)
        {
            crateLife.onBreak.RemoveListener(HandleCrateBroken);
        }
    }
}

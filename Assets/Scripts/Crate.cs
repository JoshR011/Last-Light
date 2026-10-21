/*
 * Authors: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song
 * Last updated: 2026-10-21
 * Summary: Reveals the winning crate or spawns a temporary hint when a crate breaks.
 */

using UnityEngine;

public class Crate : MonoBehaviour
{
    private const float HintLifetimeSeconds = 4f;

    [Header("Hint Prefab")]
    public GameObject hintLine;

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

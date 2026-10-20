/*
 * Authors: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song
 * Last updated: 2026-10-19
 * Summary: Reveals the winning crate or spawns a temporary hint when a crate breaks.
 */

using UnityEngine;

public class Crate : MonoBehaviour
{
    private const float HintLifetimeSeconds = 4f;

    [Header("Hint Prefab")]
    public GameObject hintLine;

    private void Awake()
    {
        // Subscribing here lets any source of damage trigger the same crate response.
        Life crateLife = GetComponent<Life>();
        crateLife.onBreak.AddListener(HandleCrateBroken);
    }

    private void HandleCrateBroken()
    {
        // The winning crate ends the search, so it does not need to reveal another hint.
        if (CrateManager.instance.winningCrate == this)
        {
            print("You found the right crate!");
        }
        else
        {
            SpawnDirectionalHint();
        }

        // Remove the broken crate so it cannot obstruct the player or produce more hints.
        Destroy(gameObject);
    }

    private void SpawnDirectionalHint()
    {
        // Collider centers keep the hint aligned even when a model's pivot is offset.
        Vector3 hintPosition = GetComponent<Collider>().bounds.center;
        Vector3 targetPosition = CrateManager.instance.winningCrate.GetComponent<Collider>().bounds.center;
        Vector3 hintDirection = targetPosition - hintPosition;

        // Align the hint's forward axis with the target instead of inheriting the crate's rotation.
        GameObject hint = Instantiate(hintLine, hintPosition, Quaternion.identity);
        hint.transform.rotation = Quaternion.LookRotation(hintDirection, hint.transform.up);
        // A short lifetime gives the player a clue without leaving permanent scene clutter.
        Destroy(hint, HintLifetimeSeconds);
    }
}

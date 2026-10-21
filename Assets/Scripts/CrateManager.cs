/*
 * Authors: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song
 * Last updated: 2026-10-21
 * Summary: Collects active crates and selects the winning crate for the scene.
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CrateManager : MonoBehaviour
{
    public static CrateManager instance;

    [Header("Scene Crates")]
    public List<Crate> crates;

    [Header("Winning Crate")]
    [FormerlySerializedAs("realCrate")]
    public Crate winningCrate;

    public bool IsSolved { get; private set; }

    private void Awake()
    {
        // Keep the first registered manager as the shared reference used by crate callbacks.
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        // Build the candidate list before choosing a winner so selection uses this scene's crates.
        CollectActiveCrates();
        SelectWinningCrate();
    }

    private void CollectActiveCrates()
    {
        // Inactive crates cannot be found by the player, and random selection needs no sorted order.
        crates = new List<Crate>(FindObjectsByType<Crate>(FindObjectsInactive.Exclude, FindObjectsSortMode.None));
    }

    private void SelectWinningCrate()
    {
        // An Inspector assignment lets a scene use a predetermined winning crate.
        if (winningCrate == null)
        {
            if (crates.Count == 0)
            {
                Debug.LogWarning("CrateManager found no active crates for the puzzle.", this);
                return;
            }

            // Select once at startup so every later hint points toward the same crate.
            int winningCrateIndex = Random.Range(0, crates.Count);
            winningCrate = crates[winningCrateIndex];
        }
    }

    public void CompletePuzzle()
    {
        // The winning crate is about to be destroyed; later breaks must not target it.
        IsSolved = true;
        winningCrate = null;
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}

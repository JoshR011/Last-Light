// Team members: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Tracks the active crates and selects the one the player needs to find.
public class CrateManager : MonoBehaviour
{
    // Share the manager and winning crate with the individual crate scripts.
    public static CrateManager instance;
    public List<Crate> crates;
    public Crate realCrate;

    void Awake()
    {
        // Register the first manager so crates can access it through the shared instance.
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        // Collect active crates in the scene without requesting a particular sort order.
        crates = new List<Crate>(FindObjectsByType<Crate>(FindObjectsInactive.Exclude, FindObjectsSortMode.None));
        
        // Choose a random winning crate unless one was already assigned in the Inspector.
        if (realCrate == null)
        {
            int index = Random.Range(0, crates.Count);
            realCrate = crates[index];
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateManager : MonoBehaviour
{
    public static CrateManager instance;
    public List<Crate> crates;
    public Crate realCrate;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        crates = new List<Crate>(FindObjectsByType<Crate>(FindObjectsInactive.Exclude, FindObjectsSortMode.None));
        
        if (realCrate == null)
        {
            int index = Random.Range(0, crates.Count);
            realCrate = crates[index];
        }
    }
}

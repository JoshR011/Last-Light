// Team members: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Stores health and notifies listeners whenever an update finds it depleted.
public class Life : MonoBehaviour
{
    // Health is changed by damage and healing scripts; onBreak defines the depletion response.
    public float amount;
    public UnityEvent onBreak;
    // No initialization is currently needed; starting health and listeners are set externally.
    void Start()
    {
        
    }

    // Invoke the break event each frame while health is zero or below.
    void Update()
    {
        if (amount <= 0)
        {
            onBreak.Invoke();
        }
    }

    // Connect this to onBreak to remove this GameObject and all of its children.
    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}

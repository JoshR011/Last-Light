using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public List<Weapon> inventory;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        inventory = GetImmediateChildrenOfType<Weapon>();
    }

    List<T> GetImmediateChildrenOfType<T>() where T : Component
    {
        var components = new List<T>();

        // Collect matching components on direct children, ignoring grandchildren.
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out T component))
            {
                components.Add(component);
            }
        }

        return components;
    }
}

using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public List<GameObject> inventory;
    public int current = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        inventory = GetImmediateChildren();
        foreach (GameObject item in inventory)
        {
            item.SetActive(false);
        }

        if (current >= 0 && current < inventory.Count) inventory[current].SetActive(true);
    }

    List<GameObject> GetImmediateChildren()
    {
        var children = new List<GameObject>();

        // Collect direct children, including inactive objects, ignoring grandchildren.
        foreach (Transform child in transform)
        {
            children.Add(child.gameObject);
        }

        return children;
    }

    void OnSlot1()
    {
        SelectSlot(0);
    }

    void OnSlot2()
    {
        SelectSlot(1);
    }

    void OnSlot3()
    {
        SelectSlot(2);
    }

    void SelectSlot(int index)
    {
        if (inventory != null && index < inventory.Count && index >= 0 && current != index)
        {
            inventory[current].SetActive(false);
            current = index;
            inventory[current].SetActive(true);
        }
    }
}


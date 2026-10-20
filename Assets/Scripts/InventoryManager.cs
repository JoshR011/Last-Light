/*
 * Authors: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song
 * Last updated: 2026-10-19
 * Summary: Builds an inventory from direct children and equips items through slot input.
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    [Header("Inventory Items")]
    public List<GameObject> inventory;

    [Header("Selected Slot")]
    [FormerlySerializedAs("current")]
    public int selectedSlotIndex = 0;

    private void Awake()
    {
        // Keep the first inventory registered as the shared reference for other components.
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        // Discover items before selecting one so slot indices follow the child order in the Hierarchy.
        inventory = CollectInventoryItems();
        InitializeSelectedItem();
    }

    private void InitializeSelectedItem()
    {
        // Prefabs may start with several items active; only the selected item should remain equipped.
        foreach (GameObject item in inventory)
        {
            item.SetActive(false);
        }

        // An empty inventory or invalid starting slot should leave every item unequipped.
        if (selectedSlotIndex >= 0 && selectedSlotIndex < inventory.Count)
        {
            inventory[selectedSlotIndex].SetActive(true);
        }
    }

    private List<GameObject> CollectInventoryItems()
    {
        List<GameObject> items = new List<GameObject>();

        // Inactive children are equipable items; grandchildren belong to those items.
        foreach (Transform child in transform)
        {
            items.Add(child.gameObject);
        }

        return items;
    }

    private void OnSlot1()
    {
        // Input action names use one-based slot numbers, while list indices start at zero.
        SelectInventorySlot(0);
    }

    private void OnSlot2()
    {
        SelectInventorySlot(1);
    }

    private void OnSlot3()
    {
        SelectInventorySlot(2);
    }

    private void SelectInventorySlot(int slotIndex)
    {
        // Ignore unavailable slots and repeated selections to avoid unnecessary item reactivation.
        if (inventory != null && slotIndex >= 0 && slotIndex < inventory.Count && selectedSlotIndex != slotIndex)
        {
            // Disable the previous item first so two equipped weapons cannot respond to input together.
            inventory[selectedSlotIndex].SetActive(false);
            selectedSlotIndex = slotIndex;
            inventory[selectedSlotIndex].SetActive(true);
        }
    }
}


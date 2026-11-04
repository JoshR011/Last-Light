/*
 * Authors: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song
 * Last updated: 2026-10-19
 * Summary: Starts with a pistol and unlocks additional weapons through ground pickups.
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum WeaponType
{
    Pistol,
    AssaultRifle,
    BurstSMG
}

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    [Header("Inventory Items")]
    public List<GameObject> inventory;

    [Header("Selected Slot")]
    [FormerlySerializedAs("current")]
    public int selectedSlotIndex = 0;

    // Keep the authored weapon transforms, but only expose weapons that have been collected.
    private readonly Dictionary<WeaponType, GameObject> availableWeapons = new Dictionary<WeaponType, GameObject>();

    private void Awake()
    {
        // Keep the first inventory registered as the shared reference for other components.
        if (instance == null)
        {
            instance = this;
        }

        inventory = new List<GameObject>();
        selectedSlotIndex = -1;
        foreach (Transform child in transform)
        {
            GameObject weapon = child.gameObject;
            weapon.SetActive(false);
            if (weapon.GetComponent<pistol>() != null)
            {
                availableWeapons[WeaponType.Pistol] = weapon;
            }
            else if (weapon.GetComponent<AutomaticWeapon>() != null)
            {
                availableWeapons[WeaponType.AssaultRifle] = weapon;
            }
            else if (weapon.GetComponent<BurstWeapon>() != null)
            {
                availableWeapons[WeaponType.BurstSMG] = weapon;
            }
        }

        if (TryCollectWeapon(WeaponType.Pistol))
        {
            SelectInventorySlot(0);
        }
        else
        {
            Debug.LogError("Inventory requires a pistol child for the starting weapon.", this);
        }
    }

    public bool TryCollectWeapon(WeaponType weaponType)
    {
        if (!availableWeapons.TryGetValue(weaponType, out GameObject weapon) || weapon == null)
        {
            return false;
        }

        // Duplicates are collected without filling another slot or changing the equipped weapon.
        if (!inventory.Contains(weapon))
        {
            inventory.Add(weapon);
        }

        return true;
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

    public void SelectInventorySlot(int slotIndex)
    {
        // Ignore unavailable slots and repeated selections to avoid unnecessary item reactivation.
        if (inventory != null && slotIndex >= 0 && slotIndex < inventory.Count && selectedSlotIndex != slotIndex)
        {
            // Disable the previous item first so two equipped weapons cannot respond to input together.
            if (selectedSlotIndex >= 0 && selectedSlotIndex < inventory.Count)
            {
                inventory[selectedSlotIndex].SetActive(false);
            }
            selectedSlotIndex = slotIndex;
            inventory[selectedSlotIndex].SetActive(true);
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}


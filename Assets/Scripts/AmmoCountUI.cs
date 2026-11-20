/*
 * Authors: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song
 * Last updated: 2026-11-19
 * Summary: Shows the equipped weapon's ammo count in the UI and updates it when the ammo changes.
 *      
 */

using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class AmmoCountUI : MonoBehaviour
{
    private TMP_Text text;

    private WeaponAmmo displayedWeapon;
    private int displayedRounds = -1;
    private int displayedReserve = -1;
    private bool displayedReloading;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        WeaponAmmo ammo = InventoryManager.EquippedAmmo;

        // Hide the text when no gun with ammo is equipped.
        text.enabled = ammo != null;
        if (ammo == null)
        {
            displayedWeapon = null;
            return;
        }

        // Skip the update when nothing changed.
        if (ammo == displayedWeapon &&
            ammo.RoundsInMagazine == displayedRounds &&
            ammo.reserveAmmo == displayedReserve &&
            ammo.IsReloading == displayedReloading)
        {
            return;
        }

        displayedWeapon = ammo;
        displayedRounds = ammo.RoundsInMagazine;
        displayedReserve = ammo.reserveAmmo;
        displayedReloading = ammo.IsReloading;

        if (displayedReloading)
        {
            text.SetText("Reloading...");
        }
        else
        {
            // SetText fills in the numbers without allocating a new string.
            text.SetText("{0} / {1}", displayedRounds, displayedReserve);
        }
    }
}
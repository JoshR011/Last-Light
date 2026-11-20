/*
 * Authors: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song
 * Date Created: 2026-11-20
 * Date Last Updated: 2026-11-20
 * Summary: Shows the equipped weapon's ammo count in the UI and updates it when the ammo changes.
 */

using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class AmmoCountUI : MonoBehaviour
{
    private TMP_Text ammoText;

    private WeaponAmmo displayedWeapon;
    private int displayedRounds = -1;
    private int displayedReserve = -1;
    private bool displayedReloading;

    private void Awake()
    {
        ammoText = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        WeaponAmmo ammo = InventoryManager.EquippedAmmo;

        ammoText.enabled = ammo != null;
        if (ammo == null)
        {
            displayedWeapon = null;
            return;
        }

        // Cache the display state so TextMeshPro only rebuilds the label when its inputs change.
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
            ammoText.SetText("Reloading...");
        }
        else
        {
            // SetText fills in the numbers without allocating a new string.
            ammoText.SetText("{0} / {1}", displayedRounds, displayedReserve);
        }
    }
}

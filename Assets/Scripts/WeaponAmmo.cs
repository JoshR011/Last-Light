/*
 * Authors: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song
 * Last updated: 2026-11-19
 * Summary: Tracks a weapon's magazine and reserve ammo and reloads when the reload input is pressed.
 *     
 */

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponAmmo : MonoBehaviour
{
    [Header("Ammo")]
    public int magazineSize = 12;
    public int reserveAmmo = 36;
    public float reloadSeconds = 1f;

    private int roundsInMagazine;
    private bool isReloading;

    private void Awake()
    {
        // Start with a full magazine.
        roundsInMagazine = magazineSize;
    }

    private void OnDisable()
    {
        // Switching weapons stops the reload coroutine, so reset the flag or the gun would stay locked.
        isReloading = false;
    }

    public bool TryUseRound()
    {
        // Weapon scripts call this right before firing; false means the shot must not happen.
        if (isReloading || roundsInMagazine <= 0)
        {
            return false;
        }

        roundsInMagazine--;
        return true;
    }

    public void OnReload(InputValue value)
    {
        // PlayerInput broadcasts Reload to the active weapon. Skip if already reloading, full, or out of spare ammo.
        if (value.isPressed && !isReloading && roundsInMagazine != magazineSize && reserveAmmo > 0)
        {
            StartCoroutine(Reload());
        }
    }

    public void AddReserveAmmo(int amount)
    {
        // For ammo pickups later.
        reserveAmmo += amount;
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadSeconds);

        // Only move as many rounds as the magazine needs and the reserve has.
        int roundsLoaded = Mathf.Min(magazineSize - roundsInMagazine, reserveAmmo);
        roundsInMagazine += roundsLoaded;
        reserveAmmo -= roundsLoaded;

        isReloading = false;
    }
}
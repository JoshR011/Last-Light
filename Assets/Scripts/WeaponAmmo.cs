/*
 * Authors: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song
 * Date Created: 2026-11-19
 * Date Last Updated: 2026-11-20
 * Summary: Tracks a weapon's magazine and reserve ammo and reloads when the reload input is pressed.
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

    // Raised every time a round is fired, so UI like the crosshair can react.
    public static event System.Action Fired;

    public int RoundsInMagazine => roundsInMagazine;
    public bool IsReloading => isReloading;

    internal bool HasRoundReady => !isReloading && roundsInMagazine > 0;

    private void Awake()
    {
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
        if (!HasRoundReady)
        {
            return false;
        }

        roundsInMagazine--;
        Fired?.Invoke();
        return true;
    }

    public void OnReload(InputValue value)
    {
        // PlayerInput broadcasts Reload to the active weapon, including on button release.
        if (value.isPressed && !isReloading && roundsInMagazine != magazineSize && reserveAmmo > 0)
        {
            StartCoroutine(ReloadMagazine());
        }
    }

    public void AddReserveAmmo(int amount)
    {
        reserveAmmo += amount;
    }

    private IEnumerator ReloadMagazine()
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

/*
 * Authors: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song
 * Last updated: 2026-11-04
 * Summary: Plays a muzzle flash at the weapon barrel and removes it after a short lifetime.
 */

using UnityEngine;

public static class WeaponMuzzleFlash
{
    public static void Play(GameObject prefab, Transform muzzle, int weaponLayer)
    {
        // Treat the flash as optional so missing effect references do not interrupt firing.
        if (prefab == null || muzzle == null)
        {
            return;
        }

        // Follow the barrel during the flash, including while the player moves or aims.
        GameObject flash = Object.Instantiate(prefab, muzzle.position,
            muzzle.rotation * prefab.transform.localRotation, muzzle);

        // Keep every particle on the weapon's layer so camera culling treats the effect consistently.
        foreach (Transform child in flash.GetComponentsInChildren<Transform>(true))
        {
            child.gameObject.layer = weaponLayer;
        }

        // Timed destruction still runs when switching weapons deactivates the muzzle.
        Object.Destroy(flash, 0.5f);
    }
}

/*
 * Authors: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song
 * Last updated: 2026-11-06
 * Summary: Fires a projectile per mouse press and removes it after its lifetime.
 */

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PistolWeapon : MonoBehaviour
{
    [Header("Projectile References")]
    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    [Header("Shot Effects")]
    public GameObject muzzleFlashPrefab;
    public AudioSource shootSound;

    [Header("Projectile Settings")]
    // Keep existing prefab tuning even though the new name reflects an impulse rather than a velocity.
    [FormerlySerializedAs("bulletVelocity")]
    public float bulletImpulse = 30f;
    [FormerlySerializedAs("bulletPrefabLifetime")]
    public float bulletLifetimeSeconds = 3f;

    private WeaponAmmo ammo;
    private FirstPersonWeapon heldWeapon;



    private void Awake()
    {
        
        ammo = GetComponent<WeaponAmmo>();
        heldWeapon = GetComponent<FirstPersonWeapon>();
    }

    private void LateUpdate()
    {
        // Check the press edge so holding the mouse button does not fire every frame.
        if (!InventoryManager.IsInventoryOpen && Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame && ammo.TryUseRound())
        {
            FireWeapon();
        }
    }

    private void FireWeapon()
    {
        Ray shot = FirstPersonWeapon.GetShot(heldWeapon, bulletSpawn);
        Quaternion rotation = Quaternion.LookRotation(shot.direction, bulletSpawn.up);
        GameObject bullet = Instantiate(bulletPrefab, shot.origin, rotation);

        // A one-time impulse launches the projectile and leaves later motion to the physics system.
        bullet.GetComponent<Rigidbody>().AddForce(shot.direction * bulletImpulse, ForceMode.Impulse);

        WeaponMuzzleFlash.Play(muzzleFlashPrefab, bulletSpawn, gameObject.layer);
        shootSound?.Play();

        // Missed shots still need a lifetime limit so they do not accumulate in the scene.
        StartCoroutine(DestroyBulletAfterDelay(bullet, bulletLifetimeSeconds));
    }

    private IEnumerator DestroyBulletAfterDelay(GameObject bullet, float lifetimeSeconds)
    {
        // Yielding lets gameplay continue during the projectile's lifetime.
        yield return new WaitForSeconds(lifetimeSeconds);

        // ContactDamager may have already destroyed the projectile when it hit something.
        if (bullet != null)
        {
            Destroy(bullet);
        }
    }
}

/*
 * Authors: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song
 * Date Created: 2026-10-07
 * Date Last Updated: 2026-11-20
 * Summary: Fires projectiles continuously at a fixed rate while the mouse button is held.
 */

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class AutomaticWeapon : MonoBehaviour
{
    [Header("Projectile References")]
    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    [Header("Shot Effects")]
    public GameObject muzzleFlashPrefab;
    public AudioSource shootSound;

    [Header("Projectile Settings")]
    public float bulletImpulse = 30f;
    public float bulletLifetimeSeconds = 3f;

    [Header("Fire Rate")]
    [Min(0.01f)]
    public float shotsPerSecond = 10f;

    // Earliest time the next shot is allowed, so the fire rate is independent of frame rate.
    private float nextFireTime;
    private WeaponAmmo ammo;
    private FirstPersonWeapon heldWeapon;

    internal bool IsReadyToFire => Time.time >= nextFireTime;

    private void Awake()
    {
      
        ammo = GetComponent<WeaponAmmo>();
        heldWeapon = GetComponent<FirstPersonWeapon>();
    }

    private void LateUpdate()
    {
        // Check the held state (not the press edge) so the weapon keeps firing while the button is down.
        if (!InventoryManager.IsInventoryOpen && Mouse.current != null && Mouse.current.leftButton.isPressed && IsReadyToFire && ammo.TryUseRound())
        {
            FireWeapon();
            nextFireTime = Time.time + 1f / shotsPerSecond;
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
        yield return new WaitForSeconds(lifetimeSeconds);

        // ContactDamager may have already destroyed the projectile when it hit something.
        if (bullet != null)
        {
            Destroy(bullet);
        }
    }
}

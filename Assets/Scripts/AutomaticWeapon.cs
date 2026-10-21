/*
 * Authors: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song
 * Last updated: 2026-10-21
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

    [Header("Projectile Settings")]
    public float bulletImpulse = 30f;
    public float bulletLifetimeSeconds = 3f;

    [Header("Fire Rate")]
    [Min(0.01f)]
    public float shotsPerSecond = 10f;

    // Earliest time the next shot is allowed, so the fire rate is independent of frame rate.
    private float nextFireTime;

    private void Update()
    {
        // Check the held state (not the press edge) so the weapon keeps firing while the button is down.
        if (Mouse.current.leftButton.isPressed && Time.time >= nextFireTime)
        {
            FireWeapon();
            nextFireTime = Time.time + 1f / shotsPerSecond;
        }
    }

    private void FireWeapon()
    {
        // Use the muzzle transform so each weapon can define its own projectile origin and aim.
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

        // A one-time impulse launches the projectile and leaves later motion to the physics system.
        bullet.GetComponent<Rigidbody>().AddForce(bulletSpawn.forward.normalized * bulletImpulse, ForceMode.Impulse);

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
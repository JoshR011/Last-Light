/*
 * Authors: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song
 * Last updated: 2026-10-21
 * Summary: Fires a short burst of projectiles per mouse press, then waits out a cooldown
 *          before the next burst.
 */

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class BurstWeapon : MonoBehaviour
{
    [Header("Projectile References")]
    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    [Header("Projectile Settings")]
    public float bulletImpulse = 30f;
    public float bulletLifetimeSeconds = 3f;


    [Header("Burst Settings")]
    [Min(1)]
    public int shotsPerBurst = 3;
    [Min(0f)]
    public float secondsBetweenBurstShots = 0.08f;
    [Min(0f)]
    public float burstCooldownSeconds = 0.3f;

    // Blocks new bursts while one is firing or cooling down.
    private bool isBursting;

    private void Update()
    {
        // Check the press edge so one press always produces exactly one burst.
        if (Mouse.current.leftButton.wasPressedThisFrame && !isBursting)
        {
            StartCoroutine(FireBurst());
        }
    }

    private IEnumerator FireBurst()
    {
        isBursting = true;

        for (int i = 0; i < shotsPerBurst; i++)
        {
            FireWeapon();
            yield return new WaitForSeconds(secondsBetweenBurstShots);
        }

        yield return new WaitForSeconds(burstCooldownSeconds);
        isBursting = false;
    }

    private void OnDisable()
    {
        // Disabling the component stops its coroutines, so reset the flag or the weapon would stay locked.
        isBursting = false;
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
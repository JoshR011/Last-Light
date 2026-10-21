/*
 * Authors: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song
 * Last updated: 2026-10-19
 * Summary: Fires a projectile per mouse press and removes it after its lifetime.
 */

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class pistol : MonoBehaviour
{
    [Header("Projectile References")]
    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    [Header("Projectile Settings")]
    // Keep existing prefab tuning even though the new name reflects an impulse rather than a velocity.
    [FormerlySerializedAs("bulletVelocity")]
    public float bulletImpulse = 30f;
    [FormerlySerializedAs("bulletPrefabLifetime")]
    public float bulletLifetimeSeconds = 3f;

    private void Update()
    {
        // Check the press edge so holding the mouse button does not fire every frame.
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            FireWeapon();
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

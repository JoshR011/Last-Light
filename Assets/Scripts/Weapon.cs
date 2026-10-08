using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;   

// Fires projectiles on a mouse click and removes them after their lifetime expires.
public class Weapon : MonoBehaviour
{
    // Configure the projectile, its spawn point, launch impulse, and lifetime in the Inspector.
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30;
    public float bulletPrefabLifetime = 3f;

    void Update()
    {
        // Fire once when the left mouse button is pressed, rather than continuously while held.
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            FireWeapon();
        }
    }

    void FireWeapon()
    {
        // Spawn the projectile at the muzzle and launch it in the muzzle's forward direction.
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

        bullet.GetComponent<Rigidbody>().AddForce(bulletSpawn.forward.normalized * bulletVelocity, ForceMode.Impulse);

        // Schedule cleanup so projectiles do not stay in the scene indefinitely.
        StartCoroutine(DestroyBullet(bullet, bulletPrefabLifetime));
    }

    IEnumerator DestroyBullet(GameObject bullet, float lifetime)
    {
        // Wait for this projectile's configured lifetime before checking whether it still exists.
        yield return new WaitForSeconds(lifetime);

        // ContactDamager may have already destroyed the projectile when it hit something.
        if (bullet != null)
        {
            Destroy(bullet);
        }
    }
}

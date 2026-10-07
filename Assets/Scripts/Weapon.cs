using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;   

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30;
    public float bulletPrefabLifetime = 3f;

    void Update()
    {
        
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            FireWeapon();
        }
    }

    void FireWeapon()
    {
        
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

        bullet.GetComponent<Rigidbody>().AddForce(bulletSpawn.forward.normalized * bulletVelocity, ForceMode.Impulse);

        StartCoroutine(DestroyBullet(bullet, bulletPrefabLifetime));
    }

    IEnumerator DestroyBullet(GameObject bullet, float lifetime)
    {
        yield return new WaitForSeconds(lifetime);

        // the bullet may already be destroyed by hitting something (ContactDamager),
        // so only destroy it if it still exists
        if (bullet != null)
        {
            Destroy(bullet);
        }
    }
}
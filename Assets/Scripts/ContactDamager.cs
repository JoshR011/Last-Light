// Team members: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song

using UnityEngine;

// Consumes this object on contact and damages objects that have a Life component.
public class ContactDamager : MonoBehaviour
{
    // Amount of health removed from the object hit by this trigger.
    public float damage;

    void OnTriggerEnter(Collider other)
    {
        // Remove this object after any trigger contact and look for health on the collider hit.
        Destroy(gameObject);

        Life life = other.GetComponent<Life>();

        // Apply damage only when the object hit has a Life component.
        if (life != null)
        {
            life.amount -= damage;
        }
    }
}

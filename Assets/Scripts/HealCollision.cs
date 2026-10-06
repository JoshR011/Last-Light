using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealCollision : MonoBehaviour
{
    public float heal;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Life life = other.GetComponent<Life>();

            if (life.amount < 100)
            {
                Destroy(gameObject);

                if (life != null)
                {
                    float amt = (life.amount <= life.amount - heal) ? heal : (100 - life.amount);
                    life.amount += amt;
                }
            }
        }
    }
}

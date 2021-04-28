using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : NetworkBehaviour
{
    [SerializeField]
    int damage = 25;

    void OnTriggerEnter(Collider other)
    {
        if (isServer)
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(damage);
            }
        }
    }
}

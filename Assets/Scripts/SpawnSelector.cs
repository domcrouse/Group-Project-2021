using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSelector : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Transform spawnPoint = GameObject.FindGameObjectWithTag("Spawn Point")?.transform ?? null;

        if (spawnPoint)
        {
            CharacterController cc = GetComponent<CharacterController>();

            if (cc != null)
            {
                cc.enabled = false;
            }

            if (isLocalPlayer) {
                transform.position = spawnPoint.position;
            }

            Destroy(spawnPoint.gameObject);

            if (cc != null)
            {
                cc.enabled = true;
            }
        }
    }
}

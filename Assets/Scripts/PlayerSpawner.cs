using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerSpawner : NetworkBehaviour
{
    public GameObject applePrefab;

    private void Update()
    {
        if (!isLocalPlayer) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            CmdSpawnApple();
        }
    }

    [Command]
    private void CmdSpawnApple()
    {
        GameObject apple = Instantiate(applePrefab, transform.position, Quaternion.identity);
        NetworkServer.Spawn(apple);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class LootSpawnerManager : MonoBehaviour
{
    public void SpawnLoot(Vector3 position, GameObject prefab)
    {
        GameObject loot = null;
        if (prefab != null)
        {
            loot = Instantiate(prefab, position, Quaternion.identity);
            NetworkServer.Spawn(loot);            
        }        
    }
}

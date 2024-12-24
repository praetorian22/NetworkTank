using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviour
{
    private DataManager dataManager;
    private MobSpawnerManager mobSpawnerManager;
    public void Awake()
    {
        dataManager = GetComponent<DataManager>();
        mobSpawnerManager = GetComponent<MobSpawnerManager>();
    }

    public void SpawnMobs(List<Transform> startPositions)
    {
        List<Transform> startPositionsRed = startPositions.Select(e => e).Where(e => e.gameObject.GetComponent<PointSpawn>().typeTank == typeTank.red).ToList();
        List<Transform> startPositionsBlue = startPositions.Select(e => e).Where(e => e.gameObject.GetComponent<PointSpawn>().typeTank == typeTank.blue).ToList();
        for (int i = 0; i < dataManager.redCount; i++)
        {
            Vector3 positionSpawn = startPositionsRed[Random.Range(0, startPositionsRed.Count)].position;
            mobSpawnerManager.SpawnTank(typeTank.red, positionSpawn, dataManager.redTankPrefab);
        }
        for (int i = 0; i < dataManager.blueCount; i++)
        {
            Vector3 positionSpawn = startPositionsBlue[Random.Range(0, startPositionsBlue.Count)].position;
            mobSpawnerManager.SpawnTank(typeTank.blue, positionSpawn, dataManager.blueTankPrefab);
        }
    }

    [Server]
    public void DestroyTank(GameObject gameObject, typeTank typeTank)
    {
        EffectManage.Instance.Explosion(gameObject.transform.position);
        NetworkServer.Destroy(gameObject);
    }
       
}

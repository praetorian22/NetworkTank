using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using System.Linq;
using System.IO;
using System;

public class GameManager : MonoBehaviour
{
    private DataManager dataManager;
    private MobSpawnerManager mobSpawnerManager;
    private LootSpawnerManager lootSpawnerManager;
    private EffectManage effectManage;
    private SaveLoad saveLoad;
    private UIManager uiManager;

    public string playerName;
    public typeTank type;

    public Action<string> changePlayerNameEvent;

    public static GameManager singleton { get; internal set; }
    public void Awake()
    {
        if (!InitializeSingleton())
            singleton = this;
        dataManager = GetComponent<DataManager>();
        mobSpawnerManager = GetComponent<MobSpawnerManager>();
        effectManage = GetComponent<EffectManage>();
        dataManager.Init();
        saveLoad = GetComponent<SaveLoad>();
        uiManager = GetComponent<UIManager>();
        lootSpawnerManager = GetComponent<LootSpawnerManager>();
    }
    private void OnEnable()
    {
        changePlayerNameEvent += ChangePlayerName;
        changePlayerNameEvent += uiManager.SetPlayerName;
    }
    private void OnDisable()
    {
        changePlayerNameEvent -= ChangePlayerName;
        changePlayerNameEvent -= uiManager.SetPlayerName;
    }
    bool InitializeSingleton()
    {
        if (singleton != null && singleton == this)
            return true;
        else
            return false;
    }
    public void FirstEnterPlayer()
    {
        if (File.Exists(Application.persistentDataPath + "/SaveData.dat"))
        {
            SaveData saveData = new SaveData();
            saveData = saveLoad.LoadData();
            if (saveData.namePlayer != "")
            {
                playerName = saveData.namePlayer;
                changePlayerNameEvent?.Invoke(playerName);
            }
        }
        else
        {
            string name = "";            
            changePlayerNameEvent?.Invoke(name);
        }
    }
    private void ChangePlayerName(string name)
    {
        SaveData saveData = new SaveData();
        saveData.namePlayer = name;
        saveLoad.SaveData(saveData);
        playerName = name;
    }
    [Server]
    public void SpawnMobs(List<Transform> startPositions)
    {
        List<Transform> startPositionsRed = startPositions.Select(e => e).Where(e => e.gameObject.GetComponent<PointSpawn>().typeTank == typeTank.red).ToList();
        List<Transform> startPositionsBlue = startPositions.Select(e => e).Where(e => e.gameObject.GetComponent<PointSpawn>().typeTank == typeTank.blue).ToList();
        for (int i = 0; i < dataManager.redCount; i++)
        {
            Vector3 positionSpawn = startPositionsRed[UnityEngine.Random.Range(0, startPositionsRed.Count)].position;
            mobSpawnerManager.SpawnTank(typeTank.red, positionSpawn, dataManager.redTankPrefab);
        }
        for (int i = 0; i < dataManager.blueCount; i++)
        {
            Vector3 positionSpawn = startPositionsBlue[UnityEngine.Random.Range(0, startPositionsBlue.Count)].position;
            mobSpawnerManager.SpawnTank(typeTank.blue, positionSpawn, dataManager.blueTankPrefab);
        }
    }
    [Server]
    public void SpawnLoot()
    {
        lootSpawnerManager.SpawnLoot(new Vector3(0f, 0f, -0.1f), dataManager.armorPrefab);
    }
    [Server]
    public void Explosion(Vector3 position, typeEffect typeEffect)
    {
        effectManage.Instantiate(dataManager.effectPrefabDict[typeEffect], position, 5, 0, 0, 0);
    }
    [Server]
    public void DestroyTank(GameObject gameObject, typeTank typeTank)
    {
        Explosion(gameObject.transform.position, typeEffect.explosion);
        NetworkServer.Destroy(gameObject);
    }
       
}

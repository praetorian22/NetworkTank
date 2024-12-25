using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;

public class UpgradeTank : NetworkBehaviour
{
    private HealthScript healthScript;
    [SerializeField] int startLevelArmor;
    [SerializeField] int startLevelEngine;
    [SerializeField] int startLevelMainCannon;

    [SyncVar(hook = nameof(SyncLevelArmor))] private int levelArmor;
    [SyncVar(hook = nameof(SyncLevelEngine))] private int levelEngine;
    [SyncVar(hook = nameof(SyncLevelMainCannon))] private int levelMainCannon;

    private void Awake()
    {
        healthScript = GetComponent<HealthScript>();        
    }
    public void Init()
    {
        SyncLevelArmor(levelArmor, startLevelArmor);
        SyncLevelEngine(levelEngine, startLevelEngine);
        SyncLevelMainCannon(levelMainCannon, startLevelMainCannon);
    }
    private void SyncLevelArmor(int oldValue, int newValue)
    {
        this.levelArmor = newValue;
        healthScript.ChangeLevelArmor(levelArmor);
    }
    private void SyncLevelEngine(int oldValue, int newValue)
    {
        this.levelEngine = newValue;
    }
    private void SyncLevelMainCannon(int oldValue, int newValue)
    {
        this.levelMainCannon = newValue;
    }

    public void UpgradeArmor()
    {
        if (levelArmor < healthScript.MaxLevelUP)
        {
            int nextLevelArmor = levelArmor + 1;
            SyncLevelArmor(levelArmor, nextLevelArmor);
        }
        
    }

    [ServerCallback]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Loot>() == null) return;
        collision.gameObject.GetComponent<Loot>().ActivateEffect(this);
        NetworkServer.Destroy(collision.gameObject);
    }
}

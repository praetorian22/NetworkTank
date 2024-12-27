using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;
using Mirror.Examples.Tanks;

public class UpgradeTank : NetworkBehaviour
{
    private HealthScript healthScript;
    private IMove move;
    private SpecialTank specialTank;
    [SerializeField] int startLevelArmor;
    [SerializeField] int startLevelEngine;
    [SerializeField] weaponType startTypeMainCannon;
    [SerializeField] bool player;
    [SerializeField] int maxLevelArmor;
    [SerializeField] int maxLevelEngine;
    [SerializeField] int maxLevelMainCannon;
    [SerializeField] List<float> koefEngimeModificationList = new List<float>();

    [SyncVar(hook = nameof(SyncLevelArmor))] private int levelArmor;
    [SyncVar(hook = nameof(SyncLevelEngine))] private int levelEngine;
    [SyncVar(hook = nameof(SyncMainCannon))] private weaponType typeMainCannon;
    [SyncVar(hook = nameof(SyncSpecial))] private typeSpecial typeSpecialLast;


    private void Awake()
    {
        healthScript = GetComponent<HealthScript>();
        move = GetComponent<IMove>();
        specialTank = GetComponent<SpecialTank>();
    }
    public void Init()
    {
        SyncLevelArmor(levelArmor, startLevelArmor);
        SyncLevelEngine(levelEngine, startLevelEngine);
        SyncMainCannon(typeMainCannon, startTypeMainCannon);
    }
    private void SyncLevelArmor(int oldValue, int newValue)
    {
        this.levelArmor = newValue;
        healthScript.ChangeLevelArmor(levelArmor);
        if (levelArmor > 0)
        {
            move.SetSpeed(levelEngine * koefEngimeModificationList[levelArmor]);            
            if (player)
            {                
                gameObject.GetComponent<GamePlayer>().SetSprite(levelArmor);
                gameObject.GetComponent<PlayerController>().ActivateCannon(levelArmor);                
            }
            else
            {
                gameObject.GetComponent<GameMob>().SetSprite(levelArmor);
                gameObject.GetComponent<EnemyShot>().ActivateCannon(levelArmor);
            }
            ActivateAnimationLootTake();
        }        
    }
    private void SyncLevelEngine(int oldValue, int newValue)
    {
        this.levelEngine = newValue;
        move.SetSpeed(levelEngine * koefEngimeModificationList[levelArmor] * 0.5f);
        ActivateAnimationLootTake();
    }
    private void SyncMainCannon(weaponType oldValue, weaponType newValue)
    {
        this.typeMainCannon = newValue;
        if (gameObject.GetComponent<GamePlayer>() != null)
        {
            foreach (WeaponScript weaponScript in gameObject.GetComponent<GamePlayer>().weaponScripts)
            {
                weaponScript.SetWeapon(GameManager.singleton.GetWeapon(newValue));
            }
        }
        if (gameObject.GetComponent<EnemyShot>() != null)
        {
            foreach (WeaponScript weaponScript in gameObject.GetComponent<EnemyShot>().WeaponScripts)
            {
                weaponScript.SetWeapon(GameManager.singleton.GetWeapon(newValue));
            }
        }
    }
    private void SyncSpecial(typeSpecial oldValue, typeSpecial newValue)
    {
        this.typeSpecialLast = newValue;
        gameObject.GetComponent<SpecialTank>().AddSpecial(typeSpecialLast);
        ActivateAnimationLootTake();
    }
    private void ActivateAnimationLootTake()
    {
        if (player)
        {
            HealthScript healthScript = gameObject.GetComponent<HealthScript>();
            if (healthScript.IsEnemy)
            {
                if (this.levelArmor < 3) gameObject.GetComponent<Animator>().SetTrigger("LR1");
                if (this.levelArmor < 5 && this.levelArmor >= 3) gameObject.GetComponent<Animator>().SetTrigger("LR2");
                if (this.levelArmor < 7 && this.levelArmor >= 5) gameObject.GetComponent<Animator>().SetTrigger("LR3");
            }
            else
            {
                if (this.levelArmor < 3) gameObject.GetComponent<Animator>().SetTrigger("LB1");
                if (this.levelArmor < 5 && this.levelArmor >= 3) gameObject.GetComponent<Animator>().SetTrigger("LB2");
                if (this.levelArmor < 7 && this.levelArmor >= 5) gameObject.GetComponent<Animator>().SetTrigger("LB3");
            }
        }
        else
        {
            if (this.levelArmor < 3) gameObject.GetComponent<Animator>().SetTrigger("L1");
            if (this.levelArmor < 5 && this.levelArmor >= 3) gameObject.GetComponent<Animator>().SetTrigger("L2");
            if (this.levelArmor < 7 && this.levelArmor >= 5) gameObject.GetComponent<Animator>().SetTrigger("L3");            
        }
    }

    public void UpgradeArmor()
    {
        if (levelArmor < maxLevelArmor)
        {
            int nextLevelArmor = levelArmor + 1;
            SyncLevelArmor(levelArmor, nextLevelArmor);
        }        
    }
    public void UpgradeEngine()
    {
        if (levelEngine < maxLevelEngine)
        {
            int nextLevelEngine = levelEngine + 1;
            SyncLevelEngine(levelEngine, nextLevelEngine);
        }
    }
    public void UpgaredWeapon(weaponType weaponType)
    {
        SyncMainCannon(weaponType.first, weaponType);
    }
    public void UpgradeSpecial(typeSpecial special)
    {
        SyncSpecial(typeSpecial.none, special);
    }

    [ServerCallback]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Loot>() == null) return;
        collision.gameObject.GetComponent<Loot>().ActivateEffect(this);
        NetworkServer.Destroy(collision.gameObject);
    }
}


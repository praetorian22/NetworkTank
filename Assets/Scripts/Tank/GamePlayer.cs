using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class GamePlayer : NetworkBehaviour
{
    [SyncVar(hook = nameof(SyncSetting))] private typeTank typeTank;
    public typeTank TypeTank => typeTank;
    public SpriteRenderer tankSpriteRenderer;
    public List<WeaponScript> weaponScripts = new List<WeaponScript>();
    public Sprite defaultSpriteRed;
    public Sprite defaultSpriteBlue;
    public Sprite spriteRed_2;
    public Sprite spriteBlue_2;
    public Sprite spriteRed_3;
    public Sprite spriteBlue_3;
    //public GameObject _shotPrefabRed;
    //public GameObject _shotPrefabBlue;    

    [Server]
    public void ChangeTypeTank(typeTank typeTank)
    {
        //this.typeTank = typeTank;
        SyncSetting(typeTank.red, typeTank);
    }     
    private void SyncSetting(typeTank oldValue, typeTank newValue)
    {
        this.typeTank = newValue;
        if (newValue == typeTank.red)
        {
            tankSpriteRenderer.sprite = defaultSpriteRed;
            foreach (WeaponScript weapon in weaponScripts)
            {
                //weapon.Init(_shotPrefabRed);
                weapon.SetPointShotPosition(newValue);
            }
        }
        else
        {
            tankSpriteRenderer.sprite = defaultSpriteBlue;
            foreach (WeaponScript weapon in weaponScripts)
            {
                //weapon.Init(_shotPrefabBlue);
                weapon.SetPointShotPosition(newValue);
            }
        }
    }
    public void SetSprite(int level)
    {
        if (typeTank == typeTank.red)
        {
            if (level < 3)
                tankSpriteRenderer.sprite = defaultSpriteRed;            
            if (level >= 3 && level < 5)
                tankSpriteRenderer.sprite = spriteRed_2;
            if (level >= 5 && level < 7)
                tankSpriteRenderer.sprite = spriteRed_3;
        }
        else
        {
            if (level < 3)
                tankSpriteRenderer.sprite = defaultSpriteBlue;
            if (level >= 3 && level < 5)
                tankSpriteRenderer.sprite = spriteBlue_2;
            if (level >= 5 && level < 7)
                tankSpriteRenderer.sprite = spriteBlue_3;
        }
    }
    public override void OnStartClient()
    {
        SyncSetting(typeTank.red, typeTank);
        base.OnStartClient();
    }
    
}

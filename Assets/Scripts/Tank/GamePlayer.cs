using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using static UnityEngine.Tilemaps.Tilemap;

public class GamePlayer : NetworkBehaviour
{
    [SyncVar(hook = nameof(SyncSetting))] private typeTank typeTank;
    public typeTank TypeTank => typeTank;
    public SpriteRenderer tankSpriteRenderer;
    public List<WeaponScript> weaponScripts = new List<WeaponScript>();
    public Sprite defaultSpriteRed;
    public Sprite defaultSpriteBlue;
    public GameObject _shotPrefabRed;
    public GameObject _shotPrefabBlue;

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
                weapon.Init(_shotPrefabRed);
                weapon.SetPointShotPosition(newValue);
            }
        }
        else
        {
            tankSpriteRenderer.sprite = defaultSpriteBlue;
            foreach (WeaponScript weapon in weaponScripts)
            {
                weapon.Init(_shotPrefabBlue);
                weapon.SetPointShotPosition(newValue);
            }
        }
    }
    public override void OnStartClient()
    {
        SyncSetting(typeTank.red, typeTank);
        GameManager.Instance.SetPlayer(gameObject);
        base.OnStartClient();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using static UnityEngine.Tilemaps.Tilemap;

public class GamePlayer : NetworkBehaviour
{
    [SyncVar(hook = nameof(SyncSetting))] public typeTank typeTank;
    public Sprite spriteForTank;
    public SpriteRenderer tankSpriteRenderer;
    public List<WeaponScript> weaponScripts = new List<WeaponScript>();
    public Sprite defaultSpriteRed;
    public Sprite defaultSpriteBlue;
    public GameObject _shotPrefabRed;
    public GameObject _shotPrefabBlue;

    [Server]
    public void ChangeTypeTank(typeTank typeTank)
    {
        this.typeTank = typeTank;
    }     
    public void SyncSetting(typeTank oldValue, typeTank newValue)
    {
        if (newValue == typeTank.red)
        {
            tankSpriteRenderer.sprite = defaultSpriteRed;
            foreach (WeaponScript weapon in weaponScripts)
            {
                weapon.Init(_shotPrefabRed);
            }
        }
        else
        {
            tankSpriteRenderer.sprite = defaultSpriteRed;
            foreach (WeaponScript weapon in weaponScripts)
            {
                weapon.Init(_shotPrefabBlue);
            }
        }
    }
}

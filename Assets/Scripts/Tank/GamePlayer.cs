using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using static UnityEngine.Tilemaps.Tilemap;

public class GamePlayer : NetworkBehaviour
{
    [SyncVar] public typeTank typeTank;
    [SyncVar(hook = nameof(SyncSprite))] public Sprite spriteForTank;
    public SpriteRenderer tankSpriteRenderer;
    
    [Server]
    public void ChangeTypeTank(typeTank typeTank)
    {
        this.typeTank = typeTank;
    }
    [Server]
    public void ChangeSpriteTank(Sprite spriteTank)
    {
        spriteForTank = spriteTank;
    }

    private void Start()
    {
        //Init(typeTank);
    }
    public void SyncSprite(Sprite oldValue, Sprite newValue)
    {
        tankSpriteRenderer.sprite = newValue;        
    }
}

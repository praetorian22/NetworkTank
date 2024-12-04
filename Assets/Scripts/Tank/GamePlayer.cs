using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using static UnityEngine.Tilemaps.Tilemap;

public class GamePlayer : NetworkBehaviour
{
    [SyncVar(hook = nameof(SyncSprite))] public typeTank typeTank;
    public Sprite spriteForTank;
    public SpriteRenderer tankSpriteRenderer;
    public Sprite defaultSpriteRed;
    public Sprite defaultSpriteBlue;

    [Server]
    public void ChangeTypeTank(typeTank typeTank)
    {
        this.typeTank = typeTank;
    }     
    public void SyncSprite(typeTank oldValue, typeTank newValue)
    {
        if (newValue == typeTank.red) tankSpriteRenderer.sprite = defaultSpriteRed;  
        else tankSpriteRenderer.sprite = defaultSpriteRed;
    }
}

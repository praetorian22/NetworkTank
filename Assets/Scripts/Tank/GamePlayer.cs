using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GamePlayer : NetworkBehaviour
{
    [SyncVar(hook = nameof(Init))] public typeTank typeTank;
    public SpriteRenderer tankSprite;
    
    [Server]
    public void ChangeTypeTank(typeTank typeTank)
    {
        this.typeTank = typeTank;
    }

    public void Init(typeTank oldvalue, typeTank newValue)
    {        
        if (newValue == typeTank.blue)
        {
            tankSprite.sprite = DataPlayer.Instance.defaultSpriteBlue;
        }
        else
        {
            tankSprite.sprite = DataPlayer.Instance.defaultSpriteRed;
        }
    }
}

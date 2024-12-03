using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayer : MonoBehaviour
{
    public typeTank typeTank;
    public SpriteRenderer tankSprite;
    public void Init(typeTank typeTank)
    {
        this.typeTank = typeTank;
        if (typeTank == typeTank.blue)
        {
            tankSprite.sprite = DataPlayer.Instance.defaultSpriteBlue;
        }
        else
        {
            tankSprite.sprite = DataPlayer.Instance.defaultSpriteRed;
        }
    }
}

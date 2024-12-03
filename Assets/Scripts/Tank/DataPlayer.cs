using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class DataPlayer : GenericSingletonClass<DataPlayer>
{
    public string playerName;
    public typeTank type;
    public Sprite defaultSpriteRed;
    public Sprite defaultSpriteBlue;
}


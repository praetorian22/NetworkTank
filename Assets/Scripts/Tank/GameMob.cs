using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMob : MonoBehaviour
{
    public SpriteRenderer tankSpriteRenderer;
    public Sprite defaultSprite;    
    public Sprite sprite_2;    
    public Sprite sprite_3;

    public void SetSprite(int level)
    {        
        if (level < 3)
            tankSpriteRenderer.sprite = defaultSprite;
        if (level >= 3 && level < 5)
            tankSpriteRenderer.sprite = sprite_2;
        if (level >= 5 && level < 7)
            tankSpriteRenderer.sprite = sprite_3;        
    }
}

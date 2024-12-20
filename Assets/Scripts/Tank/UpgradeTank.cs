using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UpgradeTank : MonoBehaviour
{
    private int _level;    
    private int _metalRemains;

    [SerializeField] private int _maxLevel;
    [SerializeField] private List<int> _levelUpPoints = new List<int>();
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private List<Sprite> _tankTextures = new List<Sprite>();

    public Action<int> upgradeEvent;

    private void AddRemains(int count)
    {
        if (_level != _maxLevel)
        {
            _metalRemains += count;
            if (_metalRemains >= _levelUpPoints[_level])
            {
                LevelUp();
            }
        }
    }
    private void LevelUp()
    {
        _level++;
        upgradeEvent?.Invoke(_level);
        _spriteRenderer.sprite = _tankTextures[_level];
    }
    private void Start()
    {
        _level = 0;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        AddRemains(collision.GetComponent<MetalRemains>().MetalRemainsCount);
        collision.GetComponent<MetalRemains>().destroyRemainsEvent?.Invoke(collision.gameObject);
    }

}

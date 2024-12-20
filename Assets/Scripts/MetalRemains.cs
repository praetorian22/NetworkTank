using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MetalRemains : MonoBehaviour
{
    private int _metalRemainsCount;
    [SerializeField] private int _metalRemainsCountMin;
    [SerializeField] private int _metalRemainsCountMax;

    public int MetalRemainsCount { get => _metalRemainsCount; }
    public Action<GameObject> destroyRemainsEvent;


    private void Start()
    {
        _metalRemainsCount = UnityEngine.Random.Range(_metalRemainsCountMin, _metalRemainsCountMax);
    }

}

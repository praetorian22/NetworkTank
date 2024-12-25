using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EffectManage;

public class DataManager : MonoBehaviour
{
    [Header("Setting game")]
    public GameObject blueTankPrefab;
    public GameObject redTankPrefab;
    public int redCount;
    public int blueCount;
    [Header("Effects")]
    public GameObject boom;
    public GameObject boomShot;
    public GameObject boomShotMini;
    public Dictionary<typeEffect, GameObject> effectPrefabDict = new Dictionary<typeEffect, GameObject>();
    //public Transform parent;

    public void Init()
    {
        effectPrefabDict.Add(typeEffect.explosion, boom);
        effectPrefabDict.Add(typeEffect.explosionMidi, boomShot);
        effectPrefabDict.Add(typeEffect.explosionMini, boomShotMini);
        //parent = GameObject.FindWithTag("parentForGameObject").transform;
    }
}
public enum typeEffect
{
    explosion,
    explosionMidi,
    explosionMini,
}
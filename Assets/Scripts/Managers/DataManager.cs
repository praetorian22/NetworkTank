using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EffectManage;

public class DataManager : MonoBehaviour
{
    public GameObject blueTankPrefab;
    public GameObject redTankPrefab;
    public int redCount;
    public int blueCount;
    public GameObject boom;
    public GameObject boomShot;
    public Dictionary<typeEffect, GameObject> effectPrefabDict = new Dictionary<typeEffect, GameObject>();
    //public Transform parent;

    public void Init()
    {
        effectPrefabDict.Add(typeEffect.explosion, boom);
        effectPrefabDict.Add(typeEffect.explosionMini, boomShot);
        //parent = GameObject.FindWithTag("parentForGameObject").transform;
    }
}
public enum typeEffect
{
    explosion,
    explosionMini,
}
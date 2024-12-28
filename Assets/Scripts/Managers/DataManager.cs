using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static EffectManage;

public class DataManager : MonoBehaviour
{
    [Header("Setting game")]
    public GameObject blueTankPrefab;
    public GameObject redTankPrefab;
    public int redCount;
    public int blueCount;
    [Header("Effects")]
    public GameObject boomBlue;
    public GameObject boomRed;
    public GameObject boomShot;
    public GameObject boomShotMiniBlue;
    public GameObject boomShotMiniRed;
    public Dictionary<typeEffect, GameObject> effectPrefabDict = new Dictionary<typeEffect, GameObject>();
    [Header("Loots")]
    public GameObject armorPrefab;
    public GameObject enginePrefab;
    public GameObject weaponFFPrefab;
    public GameObject specialInvisibilityPrefab;
    [Header("Weapons")]
    public List<Weapon> weapons = new List<Weapon>();
    public Dictionary<weaponType, Weapon> weaponsDict = new Dictionary<weaponType, Weapon>();
    
    //public Transform parent;

    public void Init()
    {
        effectPrefabDict.Add(typeEffect.explosionBlue, boomBlue);
        effectPrefabDict.Add(typeEffect.explosionRed, boomRed);
        effectPrefabDict.Add(typeEffect.explosionMidi, boomShot);
        effectPrefabDict.Add(typeEffect.explosionMiniRed, boomShotMiniRed);
        effectPrefabDict.Add(typeEffect.explosionMiniBlue, boomShotMiniBlue);
        //parent = GameObject.FindWithTag("parentForGameObject").transform;
        foreach (var weapon in weapons)
        {
            weaponsDict.Add(weapon.WeaponType, weapon);
        }
    }
}
public enum typeEffect
{
    explosionRed,
    explosionBlue,
    explosionMidi,
    explosionMiniRed,
    explosionMiniBlue,
}
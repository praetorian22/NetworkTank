using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Weapon : MonoBehaviour
{    
    [SerializeField] private weaponType weaponType;
    [SerializeField] private GameObject _shotPrefabB;
    [SerializeField] private GameObject _shotPrefabR;
    [SerializeField] private GameObject _shotPrefabRMob;
    [SerializeField] private float _timeReloadMin;
    [SerializeField] private float _timeReloadMax;

    public float TimeReload => UnityEngine.Random.Range(_timeReloadMin, _timeReloadMax);
    public GameObject ShotPrefabR => _shotPrefabR;
    public GameObject ShotPrefabB => _shotPrefabB;
    public GameObject ShotPrefabRM => _shotPrefabRMob;
    public weaponType WeaponType => weaponType;
}
public enum weaponType
{
    first,
    fastFirst,
    miniGun,
    fireGun,
    explosion,
    missle,
}
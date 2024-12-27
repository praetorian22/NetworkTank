using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponScript : NetworkBehaviour
{
    private Weapon weaponNow;
    private bool _readyToShot;
    [SerializeField] private Transform _pointToShot;
    [SerializeField] private Vector3 positionPointShotRed;
    [SerializeField] private Vector3 positionPointShotBlue;

    [SerializeField] private typeTank typeTank;
    [SerializeField] private bool mob;
    public Action<Vector3> shotEvent;    
    public void SetWeapon(Weapon weapon)
    {
        weaponNow = weapon;
    }
    private void Start()
    {
        Init();
    }

    public void SetPointShotPosition(typeTank typeTank)
    {
        this.typeTank = typeTank;
        if (typeTank == typeTank.red)
            _pointToShot.localPosition = positionPointShotRed;
        else
            _pointToShot.localPosition = positionPointShotBlue;
    }
    public void Init()
    {
        _readyToShot = true;
    }
    public IEnumerator ReloadTimer()
    {
        _readyToShot = false;
        yield return new WaitForSeconds(weaponNow.TimeReload);
        _readyToShot = true;
    }

    public void ShotNow(Quaternion rotation)
    {
        if (_readyToShot)
        {
            if (isServer)
                Shot(rotation);
            else
                CmdShot(rotation);
            shotEvent?.Invoke(gameObject.transform.position);
            StartCoroutine(ReloadTimer());
        }
    }
    [Server]
    public void Shot(Quaternion rotation)
    {
        GameObject shot = null;
        if (typeTank == typeTank.red)
        {
            if (!mob) shot = Instantiate(weaponNow.ShotPrefabR, _pointToShot.position, rotation);
            else shot = Instantiate(weaponNow.ShotPrefabRM, _pointToShot.position, rotation);
        }

        else shot = Instantiate(weaponNow.ShotPrefabB, _pointToShot.position, rotation);
        NetworkServer.Spawn(shot);
    }
    [Command(requiresAuthority = false)]
    public void CmdShot(Quaternion rotation)
    {
        Shot(rotation);
    }
}

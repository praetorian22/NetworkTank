using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class WeaponScript : NetworkBehaviour
{
    [SerializeField] private GameObject _shotPrefabB;
    [SerializeField] private GameObject _shotPrefabR;
    [SerializeField] private float _timeReloadMin;
    [SerializeField] private float _timeReloadMax;
    [SerializeField] private Transform _pointToShot;
    [SerializeField] private Vector3 positionPointShotRed;
    [SerializeField] private Vector3 positionPointShotBlue;


    private bool _readyToShot;
    private typeTank typeTank;

    public Action<Vector3> shotEvent;    

    public void SetPointShotPosition(typeTank typeTank)
    {
        this.typeTank = typeTank;
        if (typeTank == typeTank.red)
            _pointToShot.localPosition = positionPointShotRed;
        else
            _pointToShot.localPosition = positionPointShotBlue;
    }

    private void Start()
    {
        _readyToShot = true;
    }
    //public void Init(GameObject shotPrefabB, GameObject shotPrefabR)
    //{
    //    _shotPrefabB = shotPrefabB;
    //    _shotPrefabR = shotPrefabR;
    //}
    public IEnumerator ReloadTimer()
    {
        _readyToShot = false;
        yield return new WaitForSeconds(UnityEngine.Random.Range(_timeReloadMin, _timeReloadMax));
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
        if (typeTank == typeTank.red) shot = Instantiate(_shotPrefabR, _pointToShot.position, rotation);
        else shot = Instantiate(_shotPrefabB, _pointToShot.position, rotation);
        NetworkServer.Spawn(shot);        
    }
    [Command(requiresAuthority = false)]
    public void CmdShot(Quaternion rotation)
    {
        Shot(rotation);
    }    
}

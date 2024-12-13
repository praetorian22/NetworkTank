using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class WeaponScript : NetworkBehaviour
{
    private GameObject _shotPrefab;    
    [SerializeField] private float _timeReloadMin;
    [SerializeField] private float _timeReloadMax;
    [SerializeField] private Transform _pointToShot;


    private bool _readyToShot;

    public Action<Vector3> shotEvent;

    private void Start()
    {
        _readyToShot = true;
    }
    public void Init(GameObject shotPrefab)
    {
        _shotPrefab = shotPrefab;
    }
    public IEnumerator ReloadTimer()
    {
        _readyToShot = false;
        yield return new WaitForSeconds(UnityEngine.Random.Range(_timeReloadMin, _timeReloadMax));
        _readyToShot = true;
    }
    public void Shot(Quaternion rotation)
    {
        if (_readyToShot)
        {
            CmdShot(rotation);
            shotEvent?.Invoke(gameObject.transform.position);
            StartCoroutine(ReloadTimer());
        }
    }

    [Command]
    public void CmdShot(Quaternion rotation)
    {
        GameObject shot = Instantiate(_shotPrefab, _pointToShot.position, rotation);
        NetworkServer.Spawn(shot);
        //RPCCreateShot(shot);
    }
    [ClientRpc]
    public void RPCCreateShot(GameObject shot)
    {
        
    }
}

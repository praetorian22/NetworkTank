using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    [SerializeField] private GameObject _shotPrefab;
    [SerializeField] private float _timeReloadMin;
    [SerializeField] private float _timeReloadMax;
    [SerializeField] private Transform _pointToShot;


    private bool _readyToShot;

    public Action<Vector3> shotEvent;

    private void Start()
    {
        _readyToShot = true;
    }

    public IEnumerator ReloadTimer()
    {
        _readyToShot = false;
        yield return new WaitForSeconds(UnityEngine.Random.Range(_timeReloadMin, _timeReloadMax));
        _readyToShot = true;
    }

    public void Shot(Quaternion rotation, Transform parent)
    {
        if (_readyToShot)
        {
            GameObject shot = Instantiate(_shotPrefab, _pointToShot.position, rotation, parent);
            shotEvent?.Invoke(gameObject.transform.position);
            StartCoroutine(ReloadTimer());
        }
    }
}

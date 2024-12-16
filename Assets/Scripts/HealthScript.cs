using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;


public class HealthScript : NetworkBehaviour
{
    [SyncVar(hook = nameof(SyncEnemySet))] private bool _isEnemy;
    [SerializeField] private int _health;    
    [SerializeField] private List<int> _healthUp = new List<int>();
    public int Health { get => _health; }
    public bool IsEnemy { get => _isEnemy; }

    public Action<GameObject, typeTank> deadEvent;
    public Action<int> changeHealthEvent;
    public Action<Vector3> shotEvent;

    [Server]
    public void ChangeEnemySet(typeTank typeTank)
    {
        if (typeTank == typeTank.red) SyncEnemySet(false, true);
        else SyncEnemySet(false, false);
    }    
    private void SyncEnemySet(bool oldValue, bool newValue)
    {
        this._isEnemy = newValue;
    }
    public void Damage(int value)
    {
        if (_health > 0)
        {
            _health -= value;
            if (_health <= 0) deadEvent?.Invoke(gameObject, _isEnemy ? typeTank.red : typeTank.blue);
        }
        else
        {
            _health = 0;
            deadEvent?.Invoke(gameObject, _isEnemy ? typeTank.red : typeTank.blue);
        }
        changeHealthEvent?.Invoke(_health);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ShotScript shotScript = collision.GetComponent<ShotScript>();

        if (shotScript != null && shotScript.IsEnemyShot != IsEnemy && !shotScript.Dead)
        {
            Damage(shotScript.Damage);
            shotEvent?.Invoke(shotScript.gameObject.transform.position);
            shotScript.SetDead();
        }
    }
}

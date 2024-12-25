using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;


public class HealthScript : NetworkBehaviour
{
    [SyncVar(hook = nameof(SyncEnemySet))] private bool _isEnemy;
    [SyncVar(hook = nameof(SyncHealth))] private int _health;
    [SerializeField] private int _healthMax;    
    [SerializeField] private List<int> _healthUp = new List<int>();
    public int Health { get => _health; }
    public bool IsEnemy { get => _isEnemy; }

    public Action<GameObject, typeTank> deadEvent;
    public Action<int> changeHealthEvent;
    public Action<Vector3> shotEvent;
    public void Init()
    {
        SyncHealth(_health, _healthMax);        
    }
    [Server]
    public void ChangeEnemySet(typeTank typeTank)
    {
        if (typeTank == typeTank.red) SyncEnemySet(false, true);
        else SyncEnemySet(false, false);
    }
    [Server]
    public void Damage(int value)
    {
        int health = _health;
        if (health > 0)
        {            
            health -= value;
        }        
        if (health <= 0)
        {
            health = 0;
            //GameManager.Instance.DestroyTank(gameObject, _isEnemy ? typeTank.red : typeTank.blue);            
        }
        SyncHealth(_health, health);                
        //changeHealthEvent?.Invoke(_health);
    }
    [Command(requiresAuthority = false)]
    public void CmdDamage(int value)
    {
        Damage(value);
    }
    private void SyncEnemySet(bool oldValue, bool newValue)
    {
        this._isEnemy = newValue;
    }
    private void SyncHealth(int oldValue, int newValue)
    {
        this._health = newValue;
    }
    [ServerCallback]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (!isLocalPlayer) return;
        ShotScript shotScript = collision.GetComponent<ShotScript>();

        if (shotScript != null && shotScript.IsEnemyShot != IsEnemy) //&& !shotScript.Dead)
        {
            if (isServer)
            {
                Damage(shotScript.Damage);
            }
            else
            {
                //CmdDamage(shotScript.Damage);
            }            
            shotScript.SetDead();
        }
    }
}

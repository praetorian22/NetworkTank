using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ShotScript : NetworkBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private bool _isEnemyShot;
    [SerializeField] private float _timeLife;
    [SyncVar(hook = nameof(DisableShot))] private bool dead;
    public bool Dead => dead;
    public int Damage { get => _damage; }
    public bool IsEnemyShot { get => _isEnemyShot; }

    private void DisableShot(bool oldValue, bool newValue)
    {
        dead = newValue; 
        if (dead)
        {
            gameObject.SetActive(false);
        }
    }
    public void SetDead()
    {
        if (isServer)
        {
            ChangeStatusDead(true);
        }
        else
        {
            CmdChangeStatusDead(true);
        }
    }

    [Server]
    public void ChangeStatusDead(bool newValue)
    {
        DisableShot(false, newValue);
    }
    [Command(requiresAuthority = false)]
    public void CmdChangeStatusDead(bool newValue)
    {
        ChangeStatusDead(newValue);
    }
}

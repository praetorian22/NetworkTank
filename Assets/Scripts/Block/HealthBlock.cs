using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using UnityEngine.UI;

public class HealthBlock : NetworkBehaviour
{
    [SyncVar(hook = nameof(SyncHealth))] private int _health;
    [SerializeField] private int _healthMax;
    [SerializeField] private List<Image> images = new List<Image>();
    [SerializeField] private Color _colorEmpty;
    [SerializeField] private Color _colorFull;
    public int Health { get => _health; }
    private void PaintImageHealth()
    {
        for (int i = 0; i < images.Count; i++)
        {
            if (i < _health)
            {
                images[i].color = _colorFull;
            }
            else
            {
                images[i].color = _colorEmpty;
            }
        }
    }
    public override void OnStartServer()
    {
        Init();
        base.OnStartServer();
    }
    public void Init()
    {
        SyncHealth(_health, _healthMax);
    }    
    [Server]
    public void Damage(int value)
    {
        int health = _health;
        if (health > 0)
        {
            health -= value;
            gameObject.GetComponent<NetworkAnimator>().SetTrigger("getDamageBlock");
        }
        if (health <= 0)
        {
            health = 0;
            NetworkServer.Destroy(gameObject);
        }
        SyncHealth(_health, health);
        //changeHealthEvent?.Invoke(_health);
    }
    [Command(requiresAuthority = false)]
    public void CmdDamage(int value)
    {
        Damage(value);
    }    
    private void SyncHealth(int oldValue, int newValue)
    {
        this._health = newValue;
        PaintImageHealth();
    }
    [ServerCallback]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (!isLocalPlayer) return;
        ShotScript shotScript = collision.GetComponent<ShotScript>();

        if (shotScript != null) //&& !shotScript.Dead)
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

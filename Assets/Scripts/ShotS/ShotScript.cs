using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotScript : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private bool _isEnemyShot;
    [SerializeField] private float _timeLife;

    public int Damage { get => _damage; }
    public bool IsEnemyShot { get => _isEnemyShot; }

    private void Start()
    {
        Destroy(gameObject, _timeLife);
    }
}

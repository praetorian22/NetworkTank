using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MoveShot : NetworkBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private bool _blue;

    private Vector3 _movement;
    private Rigidbody2D _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();        
    }   

    private void FixedUpdate()
    {
        if (_blue) _rb.velocity = transform.up * _speed;
        else _rb.velocity = - transform.up * _speed;
    }


}

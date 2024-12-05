using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveShot : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Vector3 _movement;
    private Rigidbody2D _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }   

    private void FixedUpdate()
    {
        _rb.velocity = transform.up * _speed;
    }
}

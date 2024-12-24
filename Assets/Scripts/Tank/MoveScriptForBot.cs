using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MoveScriptForBot : NetworkBehaviour
{
    private Rigidbody2D _rb;
    private Vector2 _movement;
    private Vector2 _targetMovement;

    [SerializeField] private float _speed;
    [SerializeField] private float _timeRepositionMin;
    [SerializeField] private float _timeRepositionMax;    

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private IEnumerator Reposition()
    {
        while (true)
        {
            _targetMovement = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            yield return new WaitForSeconds(Random.Range(_timeRepositionMin, _timeRepositionMax));
        }        
    }

    private void Start()
    {
        if (isServer) StartCoroutine(Reposition());
    }

    private void Move()
    {
        if (isServer)
        {
            _movement = Vector3.Lerp(_movement, _targetMovement, 0.01f);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, Vector3.SignedAngle(Vector3.up, _movement, Vector3.forward))), 0.05f);
        }            
    }

    private void Update()
    {
        if (isServer) Move();
    }

    private void FixedUpdate()
    {
        if (isServer) _rb.velocity = _movement * _speed;        
    }    
}

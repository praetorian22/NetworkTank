using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Mirror;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{    
    private float angleCamera;
    private Control _control;
    private Rigidbody2D _rb;
    private Vector2 _movement;
    private int koefReversMove;
    private bool moveNow;
    [SerializeField] private Transform _tank;
    [SerializeField] private float _speed;
    [SerializeField] private bool mobile;

    public FixedJoystick fixedJoystick;
    public CinemachineVirtualCamera virtualCamera;

    private void Awake()
    {
        _control = new Control();
        _rb = GetComponent<Rigidbody2D>();        
    }

    private bool Move()
    {
        Vector2 move;
        if (!mobile)
            move = _control.Map.Move.ReadValue<Vector2>() * koefReversMove;
        else
            move = new Vector2(fixedJoystick.Horizontal, fixedJoystick.Vertical) * koefReversMove;
        _movement = Vector3.Lerp(_movement, move, 0.01f);
        _tank.rotation = Quaternion.Lerp(_tank.rotation, Quaternion.Euler(new Vector3(0, 0, Vector3.SignedAngle(Vector3.up, _movement * koefReversMove, Vector3.forward))), 0.05f);
        if (move.magnitude == 0) return false;
        else return true;
    }

    private void Start()
    {
        if (isLocalPlayer)
        {
            virtualCamera = GameObject.FindWithTag("vcPlayer").GetComponent<CinemachineVirtualCamera>();
            fixedJoystick = GameObject.FindWithTag("joystick").GetComponent<FixedJoystick>();
            virtualCamera.Follow = gameObject.transform;
            Init(gameObject.GetComponent<GamePlayer>().typeTank);
            virtualCamera.transform.rotation = Quaternion.AngleAxis(angleCamera, Vector3.forward);
        }        
    }

    public void Init(typeTank typeTank)
    {
        if (typeTank == typeTank.red)
        {
            angleCamera = 180f;
            koefReversMove = -1;
        } 
        else
        {
            angleCamera = 0f;
            koefReversMove = 1;
        }
    }
    private void Update()
    {
        if (Move() != moveNow)
        {
            moveNow = !moveNow;
            if (moveNow)
            {
                
            }
            else
            {
                
            }
        }
    }

    private void FixedUpdate()
    {
        _rb.velocity = _movement * _speed;        
    }

    private void OnEnable()
    {
        _control.Enable();
    }    

    private void OnDisable()
    {
        _control.Disable();
    }
}

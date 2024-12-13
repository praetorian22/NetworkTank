using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Mirror;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour, Control.IMapActions
{    
    private float angleCamera;
    private Control _control;
    private Rigidbody2D _rb;
    private Vector2 _movement;
    private int koefReversMove;
    private bool moveNow;
    private bool isShot;
    [SerializeField] private Transform _tank;
    [SerializeField] private float _speed;
    [SerializeField] private bool mobile;
    [SerializeField] private List<WeaponScript> weaponScripts = new List<WeaponScript>();

    public FixedJoystick fixedJoystick;
    public CinemachineVirtualCamera virtualCamera;

    public bool IsShot => isShot;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _control = new Control();
        _control.Map.SetCallbacks(this);
        _control.Map.Enable();
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
        if (isLocalPlayer)
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
    }

    private void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            _rb.velocity = _movement * _speed;
        }              
    }

    private void OnEnable()
    {
        ActivateCannon(0);
        if (_control != null)
            return;

        _control = new Control();
        _control.Map.SetCallbacks(this);
        _control.Map.Enable();
        
    }    

    private void OnDisable()
    {
        _control.Map.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        
    }

    public void OnShot(InputAction.CallbackContext context)
    {
        isShot = context.performed;
        if (isShot)
        {
            foreach (WeaponScript weaponScript in weaponScripts)
            {
                if (weaponScript.enabled)
                    weaponScript.Shot(_tank.rotation);
            }
        }        
    }

    private void ActivateCannon(int level)
    {
        if (level == 0)
        {
            weaponScripts[0].enabled = true;
            weaponScripts[1].enabled = false;
            weaponScripts[2].enabled = false;
            weaponScripts[3].enabled = false;
        }
        if (level == 1)
        {
            weaponScripts[3].enabled = true;
        }
        if (level == 2)
        {
            weaponScripts[0].enabled = false;
            weaponScripts[1].enabled = true;
            weaponScripts[2].enabled = true;
            weaponScripts[3].enabled = true;
        }
    }
}

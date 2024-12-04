using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Mirror;

public class PlayerController : NetworkBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    private float angleCamera;


    private void Start()
    {
        if (isLocalPlayer)
        {
            virtualCamera = GameObject.FindWithTag("vcPlayer").GetComponent<CinemachineVirtualCamera>();
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
        }        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    private float angleCamera;


    private void Start()
    {
        virtualCamera = GameObject.FindWithTag("vcPlayer").GetComponent<CinemachineVirtualCamera>();
        virtualCamera.Follow = gameObject.transform;
        virtualCamera.transform.rotation = Quaternion.AngleAxis(angleCamera, Vector3.forward);
    }

    public void Init(typeTank typeTank)
    {
        if (typeTank == typeTank.red)
        {
            angleCamera = 180f;
        }        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;

    private void Start()
    {
        virtualCamera = GameObject.FindWithTag("vcPlayer").GetComponent<CinemachineVirtualCamera>();
        virtualCamera.Follow = gameObject.transform;
    }
}

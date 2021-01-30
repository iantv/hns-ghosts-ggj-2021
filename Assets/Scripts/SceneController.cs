using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook cinemachineFreeLook;
    private string _playerTag = "HunterPlayer";

    private void Awake()
    {
        GameObject player = GameObject.FindWithTag(_playerTag);
        cinemachineFreeLook.Follow = player.transform;
        cinemachineFreeLook.LookAt = player.transform;
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

}

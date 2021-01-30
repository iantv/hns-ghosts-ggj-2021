using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook cinemachineFreeLook;

    private void Awake()
    {
        GameObject player = GameObject.FindWithTag("Player");
        cinemachineFreeLook.Follow = player.transform;
        cinemachineFreeLook.LookAt = player.transform;
    }

}

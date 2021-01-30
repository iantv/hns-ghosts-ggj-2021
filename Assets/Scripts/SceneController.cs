using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook cinemachineFreeLook;
    private string _playerTag = "HunterPlayer";

    private void Start()
    {
        GameObject player = System.Array.Find(
            GameObject.FindGameObjectsWithTag(_playerTag),
            (GameObject g) =>
            {
                Debug.Log($"game object: {g}");
                HunterPlayerController hpc = g.GetComponent<HunterPlayerController>();
                return hpc.isLocalPlayer || hpc.isClient && hpc.isServer;
            }
        );
        Debug.Log($"player: {player}");
        if (player != null)
        {
            cinemachineFreeLook.Follow = player.transform;
            cinemachineFreeLook.LookAt = player.transform;
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

}

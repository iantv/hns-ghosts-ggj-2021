using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook cinemachineFreeLook;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private CanvasGroup selectCharacterPanel;
    
    private string _playerTag = "HunterPlayer";
    
    public void SpawnHunter()
    {
        _playerTag = "HunterPlayer";
        Instantiate(Resources.Load("HunterPlayerPrefab"), spawnPosition);
        selectCharacterPanel.alpha = 0;
        selectCharacterPanel.interactable = false;
        selectCharacterPanel.blocksRaycasts = false;
        LookAtPlayer();
    }

    public void SpawnHiding()
    {
        _playerTag = "HidingPlayer";
        Instantiate(Resources.Load("HidingPlayerPrefab"), spawnPosition.position,quaternion.identity);
        selectCharacterPanel.alpha = 0;
        selectCharacterPanel.interactable = false;
        selectCharacterPanel.blocksRaycasts = false;
        LookAtPlayer();
    }


    private void LookAtPlayer()
    {
        GameObject player = GameObject.FindWithTag(_playerTag);
        cinemachineFreeLook.Follow = player.transform;
        cinemachineFreeLook.LookAt = player.transform;
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}

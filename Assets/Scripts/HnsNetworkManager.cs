using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.Networking;

public class HnsNetworkManager : NetworkManager
{
    [SerializeField] private CinemachineFreeLook cinemachineFreeLook;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private CanvasGroup selectCharacterPanel;

    public void ButtonStartServer()
    {
        StartServer();
        
        selectCharacterPanel.alpha = 0;
        selectCharacterPanel.interactable = false;
        selectCharacterPanel.blocksRaycasts = false;
        
        // Instantiate(Resources.Load("HunterPlayerPrefab"), spawnPosition);
        // LookAtPlayer("HunterPlayer");
    }

    public void ButtonStartClient()
    {
        StartClient();
    }

    public void ButtonClientReady()
    {
        // ClientScene.Ready(client.connection);

        // if (ClientScene.localPlayers.Count == 0)
        // {
            // ClientScene.AddPlayer(0);
        // }
    }

    private bool cameraInitialised = false;
    private string playerTag = "";

    private void Update()
    {
        if (!cameraInitialised && playerTag != "")
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("HidingPlayer");
            GameObject player = null;

            foreach (var p in players)
            {
                if (p.GetComponent<HidingPlayerController>().isLocalPlayer)
                {
                    player = p;
                    break;
                }
            }
            
            if (player != null)
            {
                selectCharacterPanel.alpha = 0;
                selectCharacterPanel.interactable = false;
                selectCharacterPanel.blocksRaycasts = false;
                
                cinemachineFreeLook.Follow = player.transform;
                cinemachineFreeLook.LookAt = player.transform;
        
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;  
            }
            
        }
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId,
        NetworkReader extraMessageReader)
    {
        OnServerAddPlayer(conn, playerControllerId);
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        GameObject player = (GameObject)Instantiate(Resources.Load("HidingPlayerPrefab"), spawnPosition);
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }

    // public override void OnServerReady(NetworkConnection conn)
    // {
    //     base.OnServerReady(conn);
    //     // ClientScene.AddPlayer(0);
    //
    //     // selectCharacterPanel.alpha = 0;
    //     // selectCharacterPanel.interactable = false;
    //     // selectCharacterPanel.blocksRaycasts = false;
    //     //
    //     // GameObject player = (GameObject)Instantiate(Resources.Load("HunterPlayerPrefab"), spawnPosition);
    //     // NetworkServer.AddPlayerForConnection(conn, player, 0);
    //     //
    //     // LookAtPlayer("HunterPlayer");
    // }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        playerTag = "HidingPlayer";
    }
}

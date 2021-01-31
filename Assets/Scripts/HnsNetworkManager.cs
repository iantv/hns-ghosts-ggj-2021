using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HnsNetworkManager : NetworkManager
{
    [SerializeField] private CinemachineFreeLook cinemachineFreeLook;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private CanvasGroup selectCharacterPanel;
    private InputField inputIpAddress;

    private void Start()
    {
        inputIpAddress = GameObject.Find("InputIpAddress").GetComponent<InputField>();
        inputIpAddress.text = networkAddress;
    }

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
        networkAddress = inputIpAddress.text;
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

    private GameObject FindLocalPlayerByTag(string tag)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag(tag);

        foreach (var p in players)
        {
            if (p.GetComponent<NetworkBehaviour>().isLocalPlayer)
            {
                return p;
            }
        }

        return null;
    }

    private void Update()
    {
        if (!cameraInitialised && playerTag != "")
        {
            GameObject player = FindLocalPlayerByTag("HidingPlayer");
            player = player == null ? FindLocalPlayerByTag("HunterPlayer") : player;

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
        System.Random rnd = new System.Random();
        bool isHunter = rnd.Next(2) == 0;
        string prefabName = isHunter ? "HunterPlayerPrefab" : "HidingPlayerPrefab";
        string tag = isHunter ? "HunterPlayer" : "HidingPlayer";
        GameObject player = (GameObject)Instantiate(Resources.Load(prefabName), spawnPosition);
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

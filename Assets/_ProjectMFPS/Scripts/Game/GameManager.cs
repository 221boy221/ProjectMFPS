using UnityEngine;
using System.Collections;
using Photon;
using System;

public enum GameState {
    None,
    Startup,
    MainMenu,
    Lobby,
    WaitingRoom,
    Inventory,
    InGame    
}

public class GameManager : PunBehaviour
{
    public static GameManager Instance;

    void OnEnable()
    {
        RoomManager.OnConnectedToRoomHandler += InstantiatePlayer;
    }

    void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    void OnGUI() {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }

    public void InstantiatePlayer()
    {
        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity, 0);
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer other)
    {
        Debug.Log("Other player connected with ID:" + other.ID);
    }
}

using UnityEngine;
using System.Collections;
using Photon;

public class LobbyManager : PunBehaviour {

    public static LobbyManager Instance;

    void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }
    
    public void JoinLobby() {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby() {
        Debug.Log("Succesfully joined Lobby!");
    }

}

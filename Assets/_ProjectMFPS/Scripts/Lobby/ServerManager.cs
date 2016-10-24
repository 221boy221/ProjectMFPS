using UnityEngine;
using System.Collections;
using Photon;

public class ServerManager : PunBehaviour {

    public static ServerManager Instance;

    void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        PhotonNetwork.ConnectUsingSettings("1.0");
    }

    public override void OnConnectedToPhoton() {
        base.OnConnectedToPhoton();
        Debug.Log("Connected to Photon.");
    }

    public override void OnConnectedToMaster() {
        base.OnConnectedToMaster();
        Debug.Log("Connected to the Master Server.\nReady to join a Lobby.");
    }

}

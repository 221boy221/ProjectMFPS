using UnityEngine;
using System.Collections;
using Photon;

public class RoomManager : PunBehaviour {

    public static RoomManager Instance;

    void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public void CreateRoom(string roomName) {
        PhotonNetwork.CreateRoom(roomName);
    }
    
    public void JoinRoom(string roomName) {
        PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnJoinedRoom() {
        Debug.Log("Succesfully joined room!");
    }
    

}

using UnityEngine;
using System.Collections;
using Photon;

public class RoomManager : PunBehaviour {

    public static RoomManager Instance;
    private string _roomName;

    void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public void SetRoomName(string roomName) {
        _roomName = roomName;
    }

    public void CreateRoom() {
        PhotonNetwork.CreateRoom(_roomName);
    }
    
    public void JoinRoom() {
        PhotonNetwork.JoinRoom(_roomName);
    }

    public override void OnJoinedRoom() {
        Debug.Log("Succesfully joined room!");
    }
    

}

using UnityEngine;
using System.Collections;
using Photon;

public class RoomManager : PunBehaviour {
    public delegate void OnConnectedToRoomDelegate();
    public static event OnConnectedToRoomDelegate OnConnectedToRoomHandler;

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

    public override void OnPhotonJoinRoomFailed(object[] codeAndMsg) {
        Debug.Log("ERROR " + codeAndMsg[0] + " - " + codeAndMsg[1]);
    }

    public override void OnJoinedRoom() {
        if (OnConnectedToRoomHandler == null) //if not filled in
        {
            PhotonNetwork.Disconnect();
            return;
        }

        OnConnectedToRoomHandler();
        Debug.Log("Succesfully joined room!");
    }
    

}

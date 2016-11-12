using UnityEngine;
using System.Collections;
using Photon;
using UnityEngine.Events;

public enum GameState {
    None,
    Startup,
    MainMenu,
    Lobby,
    WaitingRoom,
    Inventory,
    InGame    
}

public class GameManager : PunBehaviour {

    public static GameManager Instance;
    public UnityAction InitializeGame;
    public UnityAction LoadedGame;

    void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        StartCoroutine(InitGame());
    }

    private IEnumerator InitGame() {
        InitializeGame();

        yield break;
    }


    void OnEnable()
    {
        RoomManager.OnConnectedToRoomHandler += InstantiatePlayer;
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

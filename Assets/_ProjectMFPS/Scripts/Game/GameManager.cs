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
    public static UnityAction InitializeGame = delegate { };
    public static UnityAction LoadedGame = delegate { };

    void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    void Start()
    {
        StartCoroutine(InitGame());
    }

    private IEnumerator InitGame() {
        Debug.Log("InitGame");
        InitializeGame();

        // fake loading, Todo!
        yield return new WaitForSeconds(2f);
        Debug.Log("LoadedGame");
        LoadedGame();

        yield break;
    }

    //void OnEnable()
    //{
    //    RoomManager.OnConnectedToRoomHandler += InstantiatePlayer;
    //}

    //public void InstantiatePlayer()
    //{
    //    PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity, 0);
    //}

    //public override void OnPhotonPlayerConnected(PhotonPlayer other)
    //{
    //    Debug.Log("Other player connected with ID:" + other.ID);
    //}

}

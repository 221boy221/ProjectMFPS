using UnityEngine;
using System.Collections;
using Photon;
using System.Collections.Generic;

public class ServerManager : PunBehaviour {

    public static ServerManager Instance;

    [SerializeField]
    private ServerList _serverList;
    private CloudRegionCode _activeRegion;

    void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        PhotonNetwork.logLevel = PhotonLogLevel.Full;
        PhotonNetwork.autoJoinLobby = false;
    }

    /// <summary>Called by Unity when the application is closed. Tries to disconnect.</summary>
    protected void OnApplicationQuit() {
        PhotonNetwork.Disconnect();
    }

    void OnGUI() {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }

    // on login:
    // PhotonNetwork.networkingPeer.AppId = PhotonNetwork.PhotonServerSettings.AppID;
    // PhotonNetwork.networkingPeer.ConnectToNameserver();

    void Start() {
        Debug.Log("Start");
        PhotonNetwork.networkingPeer.AppId = PhotonNetwork.PhotonServerSettings.AppID;
        StartCoroutine(WaitForConnnection());
    }

    private IEnumerator WaitForConnnection() {
        Debug.Log("> Waiting for connection...");
        yield return new WaitUntil(() => PhotonNetwork.networkingPeer.ConnectToNameServer() == true);
        yield return new WaitUntil(() => PhotonNetwork.networkingPeer.State == ClientState.ConnectedToNameServer);
        Debug.Log("> Connection Established!");
        StartCoroutine(RefreshServerList());
        yield break;
    }

    // On Connected to the Photon Nameserver by AppID
    public override void OnConnectedToPhoton() {
        base.OnConnectedToPhoton();
        Debug.Log("Connected to Photon.");
    }

    public override void OnConnectionFail(DisconnectCause cause) {
        base.OnConnectionFail(cause);
        Debug.LogError(cause);
    }
    public override void OnFailedToConnectToPhoton(DisconnectCause cause) {
        base.OnFailedToConnectToPhoton(cause);
        Debug.LogError(cause);
    }
    

    public IEnumerator RefreshServerList() {
        // Get list of Available Regions
       // Debug.Log("Getting list of available regions...");
        //yield return new WaitUntil(() => PhotonNetwork.networkingPeer.OpGetRegions(PhotonNetwork.networkingPeer.AppId) == true);
       // Debug.Log("Done.");
        Debug.Log("Pinging available regions...");
        yield return PhotonHandler.SP.PingAvailableRegionsCoroutine(false);
        Debug.Log("Done.");

        if (PhotonNetwork.networkingPeer.AvailableRegions != null && PhotonNetwork.networkingPeer.AvailableRegions.Count > 0) {
            _serverList.Refresh();
        }

        yield break;
    }

    public void JoinServer(uint region) {
        //if (PhotonNetwork.networkingPeer.State == ClientState.ConnectedToNameServer)
        //    PhotonNetwork.networkingPeer.Disconnect();

        _activeRegion = (CloudRegionCode)region;
        PhotonNetwork.networkingPeer.ConnectToRegionMaster((CloudRegionCode)region);

        //PhotonNetwork.ConnectToRegion((CloudRegionCode)region, GameSettings.ClientVersion);
    }

    // On Connected to a Region
    public override void OnConnectedToMaster() {
        base.OnConnectedToMaster();
        Debug.Log("Connected to the Master Server on Region '" + _activeRegion.ToString() + "'.\nReady to join a Lobby.");
    } 

}

using UnityEngine;

public class LobbyPanel : UIPanel
{

    [SerializeField] private CenterPanel _centerPanel;
    [SerializeField] private GameObject _sidePanel;


    /// <summary>
    /// Photon Callback that gets fired once the client successfully joins his selected room. Once received, we open the Waiting Room UI panel.
    /// </summary>
    public void OnJoinedRoom()
    {
        _centerPanel.OpenWaitingRoom();
    }

    private void OnEnable()
    {
        _centerPanel.gameObject.SetActive(true);
        _sidePanel.gameObject.SetActive(true);
        _centerPanel.OpenRoomList();
    }

    private void OnDisable()
    {
        _centerPanel.gameObject.SetActive(false);
        _sidePanel.gameObject.SetActive(false);
    }
}

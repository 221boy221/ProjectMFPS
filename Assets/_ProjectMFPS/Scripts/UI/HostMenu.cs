using UnityEngine;
using UnityEngine.UI;

public class HostMenu : UIPanel
{

    #region Vars
    [SerializeField] private Button _privateButton;
    [SerializeField] private Button _publicButton;
    [SerializeField] private Button _backButton;
    #endregion

    #region Methods
    private void Awake()
    {
        _privateButton.onClick.AddListener(CreatePrivateRoom);
        _publicButton.onClick.AddListener(CreatePublicRoom);
        _backButton.onClick.AddListener(OnClickedBack);
    }

    /// <summary>
    /// Opens the UI panel MainMenu.
    /// </summary>
    private void OnClickedBack() { OpenUIPanel(UIPanelTypes.MainMenu); }

    /// <summary>
    /// Calls the NetworkManager.CreateRoom() with RoomType.Public
    /// </summary>
    private void CreatePublicRoom()
    {
        NetworkManager.Instance.CreateRoom(RoomTypes.Public);
    }

    /// <summary>
    /// Calls the NetworkManager.CreateRoom() with RoomType.Private
    /// </summary>
    private void CreatePrivateRoom()
    {
        NetworkManager.Instance.CreateRoom(RoomTypes.Private);
    }

    /// <summary>
    /// Photon Callback that gets fired once the client successfully joins his newly created room. Once received, we open the Waiting Room UI panel.
    /// </summary>
    public void OnJoinedRoom()
    {
        OpenUIPanel(UIPanelTypes.WaitingRoom);
    }
    #endregion

}

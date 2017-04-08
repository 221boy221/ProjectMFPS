using UnityEngine;
using UnityEngine.UI;

public class CenterPanel : MonoBehaviour
{

    #region Vars
    private Text _headerText;

    [SerializeField] private RoomList _roomList;
    [SerializeField] private WaitingRoom _waitingRoom;
    

    #endregion

    #region Methods
    private void OnEnable()
    {
        
    }


    public void OpenRoomList()
    {
        _roomList.gameObject.SetActive(true);
        _roomList.Toggle(true);
        _waitingRoom.gameObject.SetActive(false);
        _waitingRoom.Toggle(false);
    }

    public void OpenWaitingRoom()
    {
        _waitingRoom.gameObject.SetActive(true);
        _waitingRoom.Toggle(true);
        _roomList.gameObject.SetActive(false);
        _roomList.Toggle(false);
    }
    

   

    #endregion
}

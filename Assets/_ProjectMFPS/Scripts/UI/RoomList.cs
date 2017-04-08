using System;
using UnityEngine;
using UnityEngine.UI;

public class RoomList : UIPanel
{

    #region Vars
    [SerializeField] private GameObject _roomListMenu;
    [SerializeField] private Button _quickJoinButton;
    [SerializeField] private Button _createButton;
    [SerializeField] private Button _refreshButton;
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _previousButton;
    #endregion

    #region Methods
    public override void Toggle(bool value)
    {
        _roomListMenu.SetActive(value);
    }

    protected override void SetListeners(bool on)
    {
        if (on)
        {
            _quickJoinButton.onClick.AddListener(OnClickedQuickJoin);
            _createButton.onClick.AddListener(OnClickedCreate);
            _refreshButton.onClick.AddListener(OnClickedRefresh);
            _nextButton.onClick.AddListener(OnClickedNext);
            _previousButton.onClick.AddListener(OnClickedPrevious);
        }
        else
        {
            _quickJoinButton.onClick.RemoveListener(OnClickedQuickJoin);
            _createButton.onClick.RemoveListener(OnClickedCreate);
            _refreshButton.onClick.RemoveListener(OnClickedRefresh);
            _nextButton.onClick.RemoveListener(OnClickedNext);
            _previousButton.onClick.RemoveListener(OnClickedPrevious);
        }
    }
    

    internal void OnClickedQuickJoin()
    {
        throw new NotImplementedException();
    }

    internal void OnClickedCreate()
    {
        throw new NotImplementedException();
    }

    internal void OnClickedRefresh()
    {
        throw new NotImplementedException();
    }

    internal void OnClickedNext()
    {
        throw new NotImplementedException();
    }

    internal void OnClickedPrevious()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Calls the NetworkManager.JoinRandomRoom().
    /// </summary>
    private void JoinRandomRoom()
    {
        NetworkManager.Instance.JoinRandomRoom();
    }


    #endregion

}

﻿using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    #region Vars
    [SerializeField] private UIPanel[] _UIPanels;
    #endregion
    
    #region Methods
    private void Awake()
    {
        // Entry
        AddEventListeners();
    }

    private void OnEnable()
    {
        // Check if the Player is already connected to a Room.
        if (!PhotonNetwork.inRoom)
        {
            SwitchPanelTo(UIPanelTypes.MainMenu);
        }
        else
        {
            ReconnectWithWaitingRoom();
        }
    }

    /// <summary>
    /// Switches the UI Panel back towards the Waiting Room since the client is already connected, thus skipping the Join/Host panel.
    /// </summary>
    private void ReconnectWithWaitingRoom()
    {
        NetworkManager.Instance.SetDefaultPlayerProperties(false); // Todo: check if necessary
        SwitchPanelTo(UIPanelTypes.WaitingRoom);
    }

    // Event Listeners //
    private void AddEventListeners()
    {
        for (int i = 0; i < _UIPanels.Length; i++)
        {
            _UIPanels[i].OpenUIPanelEvent += SwitchPanelTo;
        }

        ServerManager.Instance.OnConnectedToServer += OnConnectedToServer;
    }

    private void OnConnectedToServer()
    {
        ServerManager.Instance.OnConnectedToServer -= OnConnectedToServer;
        SwitchPanelTo(UIPanelTypes.Lobby);
    }

    private void RemoveEventListeners()
    {
        for (int i = 0; i < _UIPanels.Length; i++)
        {
            _UIPanels[i].OpenUIPanelEvent -= SwitchPanelTo;
        }
        
    }

    /// <summary>
    /// Disables all the UI Panels and enables the given one.
    /// </summary>
    /// <param name="panelType"></param>
    private void SwitchPanelTo(UIPanelTypes panelType)
    {
        // Disable all menus, enable given panelType
        for (int i = 0; i < _UIPanels.Length; i++)
        {
            if (_UIPanels[i].panelType == panelType)
            {
                _UIPanels[i].enabled = true;
            }
            else
            {
                _UIPanels[i].enabled = false;
            }
        }
    }
    #endregion

}

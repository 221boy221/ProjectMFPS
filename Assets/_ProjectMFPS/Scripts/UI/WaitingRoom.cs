﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WaitingRoom : UIPanel
{

    #region Vars
    [SerializeField] private GameObject _playerSlotPrefab;
    [SerializeField] private RectTransform _playerSlotHolder;

    [SerializeField] private GameObject _waitingRoomMenu;
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _readyButton;
    [SerializeField] private Button _inviteButton;
    [SerializeField] private Button _userLimitButton;
    [SerializeField] private Button _kickButton;
    [SerializeField] private Button _exitButton;

    Dictionary<int, PlayerSlot> _slots = new Dictionary<int, PlayerSlot>();
    #endregion

    #region Methods
    public override void Toggle(bool value)
    {
        _waitingRoomMenu.SetActive(value);
    }

    protected override void SetListeners(bool on)
    {
        if (on)
        {
            _startButton.onClick.AddListener(OnClickedStart);
            _readyButton.onClick.AddListener(OnClickedReady);
            _inviteButton.onClick.AddListener(OnClickedInvite);
            _userLimitButton.onClick.AddListener(OnClickedUserLimit);
            _kickButton.onClick.AddListener(OnClickedKick);
            _exitButton.onClick.AddListener(OnClickedExit);
        }
        else
        {
            _startButton.onClick.RemoveListener(OnClickedStart);
            _readyButton.onClick.RemoveListener(OnClickedReady);
            _inviteButton.onClick.RemoveListener(OnClickedInvite);
            _userLimitButton.onClick.RemoveListener(OnClickedUserLimit);
            _kickButton.onClick.RemoveListener(OnClickedKick);
            _exitButton.onClick.RemoveListener(OnClickedExit);
        }
    }

    private void OnClickedInvite() {  }
    private void OnClickedUserLimit() {  }
    private void OnClickedKick() {  }
    private void OnClickedExit() { PhotonNetwork.LeaveRoom(); OpenUIPanel(UIPanelTypes.MainMenu); }

    /// <summary>
    /// Upon done joining/creating, this script enables. Thus running this method which sets up the ui and required player properties.
    /// </summary>
    private void OnEnable()
    {
        CleanUpList();
        SetClientUI();
        UpdateUI();
    }

    /// <summary>
    /// Changes the Ready button into a Start button if to the player is a master and the other way around.
    /// </summary>
    private void SetClientUI()
    {
        if (PhotonNetwork.player.IsMasterClient)
        {
            _startButton.gameObject.SetActive(true);
            _readyButton.gameObject.SetActive(false);
        }
        else
        {
            _startButton.gameObject.SetActive(false);
            _readyButton.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// On clicked Ready button behaviour. Switches state of player custom properties and refreshes the player slot.
    /// </summary>
    private void OnClickedReady()
    {
        bool currentReadyState = (bool)PhotonNetwork.player.CustomProperties[PlayerProperties.READY_STATE];
        bool newReadyState = !currentReadyState;

        ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable
        {
            { PlayerProperties.READY_STATE, newReadyState }
        };

        PhotonNetwork.player.SetCustomProperties(propertiesToSet);
        _slots[PhotonNetwork.player.ID].RefreshData(PhotonNetwork.player);
    }

    /// <summary>
    /// On clicked Start button behaviour. Checks the states of all players and starts the game.
    /// </summary>
    private void OnClickedStart()
    {
        if (PhotonNetwork.room.PlayerCount < PhotonNetwork.room.MaxPlayers)
        {
            Debug.Log("You need 4 players in order to start! [" + PhotonNetwork.room.PlayerCount + "/" + PhotonNetwork.room.MaxPlayers+ "]");
            return;
        }

        foreach (PhotonPlayer player in PhotonNetwork.playerList)
        {
            // If player is host, skip
            if (player.IsLocal)
                continue;

            // If one of the players is not ready
            if (!(bool)player.CustomProperties[PlayerProperties.READY_STATE])
            {
                Debug.Log("Not all players are ready!");
                return;
            }
        }

        Debug.Log("All players are ready, starting game...");

        // Loading level, reset player ready states so we can use them to check if player is done loading
        foreach (PhotonPlayer player in PhotonNetwork.playerList)
        {
            player.CustomProperties[PlayerProperties.READY_STATE] = false;
        }

        PhotonNetwork.LoadLevel("GamePlay"); // Todo: replace with list of different maps (scenes) and consts
    }

    /// <summary>
    /// Updates the UI listing, it creates the necessary items not yet listed, update existing items and remove unused entries
    /// </summary>
    public void UpdateUI()
    {
        List<int> processedIDs = new List<int>();

        // Update existing slots and add new ones
        foreach (PhotonPlayer player in PhotonNetwork.playerList)
        {   
            Debug.Log("Player " + player.NickName + " ReadyState[" + player.CustomProperties[PlayerProperties.READY_STATE] + "]");

            // Update existing slot
            if (_slots.ContainsKey(player.ID))
            {
                // Found ID, refresh data
                _slots[player.ID].RefreshData(player);

                // Add ID to processed IDs list
                processedIDs.Add(player.ID);

                Debug.Log("Player " + player.NickName + " has a playerslot, refreshing...");
            }
            // Create new slot
            else
            {
                // Instantiate prefab and set data
                GameObject slot = Instantiate(_playerSlotPrefab, _playerSlotHolder);
                slot.transform.localScale = Vector3.one;
                slot.transform.localPosition = new Vector3(slot.transform.localPosition.x, slot.transform.localPosition.y, 0);

                PlayerSlot playerSlot = slot.GetComponent<PlayerSlot>();
                playerSlot.RefreshData(player);

                // Add to dictionary
                _slots.Add(player.ID, playerSlot);

                // Add ID to processed IDs list
                processedIDs.Add(player.ID);

                Debug.Log("Creating new player slot for player: " + player.NickName);
            }
        }

        // Remove playerslots of players that are no longer connected to the room
        foreach (var slot in _slots.Reverse())
        {
            if (!processedIDs.Contains(slot.Key))
            {
                _slots.Remove(slot.Key);
                Destroy(slot.Value.gameObject);
            }
        }
    }
    
    /// <summary>
    /// Replace slots dictionary with a new one and remove the remaining player slot objects
    /// </summary>
    public void CleanUpList()
    {
        _slots = new Dictionary<int, PlayerSlot>();
        foreach (RectTransform child in _playerSlotHolder)
        {
            Destroy(child.gameObject);
        }
    }

    #endregion


    #region PHOTON CALLBACKS
    public void OnJoinedRoom()
    {
        Debug.Log("You have joined the room.");
        // - 
    }

    public void OnLeftRoom()
    {
        Debug.Log("You have left the room.");

        // - 
        CleanUpList();
        OpenUIPanel(UIPanelTypes.MainMenu);
    }

    public void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        Debug.Log("Player [#" + player.ID + "] " + player.NickName + "' connected.");
        UpdateUI();
    }

    public void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        Debug.Log("Player [#" + player.ID + "] " + player.NickName + "' disconnected.");
        UpdateUI();
    }

    public void OnMasterClientSwitched()
    {
        UpdateUI();
        SetClientUI();
    }

    public void OnPhotonPlayerPropertiesChanged()
    {
        UpdateUI();
    }
    #endregion
}

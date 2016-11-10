using UnityEngine;
using System.Collections;
using Photon;
using System.Collections.Generic;

public class ServerList : PunBehaviour {

    [SerializeField] private RectTransform _serverSlotGrid;
    [SerializeField] private GameObject _serverSlotPrefab;
    private List<ServerSlot> _serverSlots;
    
    void Start() {
        _serverSlots = new List<ServerSlot>();
    }

    public void Refresh() {
        List<Region> availableRegions = PhotonNetwork.networkingPeer.AvailableRegions;
        for (int i = 0; i < availableRegions.Count; i++) {
            for (int j = 0; j < _serverSlots.Count; j++) {
                // Check if the Slot already exists
                if (_serverSlots[j].name == availableRegions[i].Code.ToString()) {
                    RefreshServerSlot(j, availableRegions[i]);
                    break;
                }
            }
            // Otherwise create a new one
            CreateServerSlot(availableRegions[i]);
        }
    }

    private void CreateServerSlot(Region region) {
        // Create Obj
        GameObject serverSlotObj = Instantiate(_serverSlotPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        serverSlotObj.transform.SetParent(_serverSlotGrid, false);
        // Set values
        ServerSlot serverSlot = serverSlotObj.GetComponent<ServerSlot>();
        serverSlot.ServerName = region.Code.ToString();
        serverSlot.RegionCode = (uint)region.Code;
        serverSlot.Ping = region.Ping;
        // Cache
        _serverSlots.Add(serverSlot);
    }

    private void RefreshServerSlot(int i, Region region) {
        _serverSlots[i].ServerName = region.Code.ToString();
        _serverSlots[i].RegionCode = (uint)region.Code;
        _serverSlots[i].Ping = region.Ping;
    }


}

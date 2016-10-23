using System;
using UnityEngine;

public class PlayerShoot : MonoBehaviour {

    private const string PLAYER_TAG = "Player";

    [SerializeField] private LayerMask _layerMask;
	[SerializeField] private Camera _camera;
    // Weapon 
    [SerializeField] private PlayerWeapon equipedWeapon; // Replace with player equipment class
    [SerializeField] private GameObject weaponGFX; // Model of equiped weapon... replace with ^
    [SerializeField] private string weaponLayerName = "Weapon";

    void Start() {
        if(_camera == null) {
            Debug.Log("PlayerShoot: No camera referenced!");
            this.enabled = false;
        }

        // Give the weapon model the right layer name so that the raycast can ignore them
        //weaponGFX.layer = LayerMask.NameToLayer(weaponLayerName);
        //foreach (Transform child in weaponGFX.transform) {
        //    child.gameObject.layer = LayerMask.NameToLayer(weaponLayerName);
        //}

    }

    void Update() {
        if (Input.GetButtonDown("Fire1")) {
            Shoot();
        }
    }

    private void Shoot() {
        RaycastHit hit;
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, equipedWeapon.range, _layerMask)) {
            // Hit
            if (hit.collider.tag == PLAYER_TAG) {
                // Sends the netID of the object we hit
                CmdPlayerShot(hit.collider.name, equipedWeapon.damage);
            }
        }
    }

    void CmdPlayerShot(string playerID, float damage) {
        Debug.Log(playerID + " has been shot.");

        //Player player = GameManager.GetPlayer(playerID);
        //player.RpcTakeDamage(damage);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerName : MonoBehaviour
{
    #region Vars

    [SerializeField]
    private Text _playerName;

    #endregion

    #region Methods

    private void Start()
    {
        _playerName.text = transform.root.GetComponent<PhotonView>().owner.NickName;
    }

    #endregion
}

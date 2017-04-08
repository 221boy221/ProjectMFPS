using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    #region Vars

    [SerializeField]
    private GameObject _graphic;

    [SerializeField]
    private float _closeDelay = 5f;
    [SerializeField]
    private bool _isOpen;

    #endregion

    #region Methods

    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(_isOpen);
        }
        else
        {
            bool isOpen = (bool)stream.ReceiveNext();
            if(isOpen)
            {
                Open();
            }
            else if(!isOpen)
            {
                Close();
            }
        }
    }

    private void Start()
    {
        Open();
    }

    private void Open()
    {
        _isOpen = true;
        _graphic.SetActive(false);
    }

    public void Close()
    {
        if (!_isOpen)
        {
            return;
        }
        _isOpen = false;
        _graphic.SetActive(true);
        StartCoroutine(AutoOpen());
    }

    private IEnumerator AutoOpen()
    {
        yield return new WaitForSeconds(_closeDelay);
        Open();
    }

    #endregion
}
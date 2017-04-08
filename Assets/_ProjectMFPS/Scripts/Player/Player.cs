using UnityEngine;

public class Player : MonoBehaviour
{
    #region Vars

    [SerializeField]
    private PlayerRoles _role;

    [SerializeField]
    private MonoBehaviour[] _playerScripts;

    [SerializeField]
    private Transform _cameraTarget;
    
    #endregion

    #region Methods
    
    private void Start()
    {
        PhotonView photonview = gameObject.GetComponent<PhotonView>();
        SetActivePlayer(photonview.isMine);
    }

    public void SetActivePlayer(bool active)
    {
        if (active)
        {
            Camera.main.transform.SetParent(_cameraTarget, false);
            Vector3 pos = Camera.main.transform.localPosition;
            pos.x = pos.y = 0;
            Camera.main.transform.localPosition = pos;
            GUIMenu.Instance.SetRole(_role);
        }
        else
        {
            foreach (MonoBehaviour script in _playerScripts)
            {
                Destroy(script);
            }
            //Destroy(GetComponent<Rigidbody2D>());
            Rigidbody2D rigid = GetComponent<Rigidbody2D>();
            if (rigid != null)
            {
                rigid.gravityScale = 0f;
            }
            Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
            foreach (Collider2D collider in colliders)
            {
                Destroy(collider);
            }
        }
    }

    #endregion

    #region Properties
    
    public PlayerRoles Role
    {
        get
        {
            return _role;
        }
    }

    #endregion
}
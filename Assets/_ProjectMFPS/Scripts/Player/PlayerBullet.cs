using System.Collections;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    #region Vars

    [SerializeField]
    private float _speed = 20f;
    [SerializeField]
    private Rigidbody2D _rigid;
    private PhotonView _photonView;

    #endregion

    #region Methods

    private void Start()
    {
        _photonView = gameObject.GetComponent<PhotonView>();
        DestroyIn(2f);
    }

    private void FixedUpdate()
    {
        _rigid.velocity = transform.up * _speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            GameHandler.Instance.BulletHitPlayer(player);
            if (_photonView.isMine)
            {
                PhotonNetwork.Destroy(_photonView);
            }
        }
    }

    private IEnumerator DestroyIn(float time)
    {
        yield return new WaitForSeconds(time);

        if (_photonView.isMine)
        {
            PhotonNetwork.Destroy(_photonView);
        }
    }

    #endregion
}

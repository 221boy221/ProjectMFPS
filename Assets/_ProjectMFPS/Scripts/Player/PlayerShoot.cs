using System.Collections;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    #region Vars

    [SerializeField]
    private float _shootInterval = 1f;

    [SerializeField]
    private GameObject _bulletPrefab;

    [SerializeField]
    private Transform _bulletSpawn;

    private bool _canShoot = true;
    private bool _shootDown = false;

    #endregion

    #region Methods

    private void Start()
    {
        InputHandler.Instance.OnInteractionDown += ShootDown;
        InputHandler.Instance.OnInteractionUp += ShootUp;
    }

    public void OnDestroy()
    {
        InputHandler.Instance.OnInteractionDown -= ShootDown;
        InputHandler.Instance.OnInteractionUp -= ShootUp;
    }

    private void ShootDown()
    {
        _shootDown = true;
        Shoot();
    }

    private void ShootUp()
    {
        _shootDown = false;
    }

    private void Shoot()
    {
        if (!_canShoot)
        {
            return;
        }
        _canShoot = false;
        StartCoroutine(ShootCooldown());
        GameObject bullet = PhotonNetwork.Instantiate(_bulletPrefab.name, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0), 0);
        bullet.transform.position = _bulletSpawn.position;
        bullet.transform.rotation = _bulletSpawn.rotation;
    }

    private IEnumerator ShootCooldown()
    {
        yield return new WaitForSeconds(_shootInterval);
        _canShoot = true;
        if (_shootDown)
        {
            Shoot();
        }
    }

    #endregion
}
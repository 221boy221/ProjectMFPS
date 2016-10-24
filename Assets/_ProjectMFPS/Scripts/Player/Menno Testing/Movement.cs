using UnityEngine;
using System.Collections;

public class Movement : Photon.MonoBehaviour {
    private const float SPEED = 3;

    [SerializeField]
    private Rigidbody _rb;
    [SerializeField]
    private Camera _camera;

    private Vector3 _rotation;
    private float _mouseSensitivity = 5;
    private float _currentCamRotX;
    private float _cameraRotationX;
    private float _camRotLimit = 85f;

    void Update ()
    {
        if(photonView.isMine)
        {
            CalcRotation();
            UpdateRotation();
            UpdateMovement();
        }
	}

    /// <summary>
    /// Calculate mouse rotation
    /// </summary>
    internal void CalcRotation()
    {
        // Calc X rotation
        float yRot = Input.GetAxisRaw("Mouse X");
        Vector3 rotation = new Vector3(0f, yRot, 0f) * _mouseSensitivity;
        _rotation = rotation;

        // Calc Y rotation
        float xRot = Input.GetAxisRaw("Mouse Y");
        float cameraRotationX = xRot * _mouseSensitivity;
        _cameraRotationX = cameraRotationX;
    }

    /// <summary>
    /// Applies calculated rotation
    /// </summary>
    private void UpdateRotation()
    {
        // Updates PlayerModel X,Y,Z rotation
        _rb.MoveRotation(_rb.rotation * Quaternion.Euler(_rotation));

        // Updates Camera X rotation
        if (_camera != null)
        {
            // Set rotation and clamp it
            _currentCamRotX -= _cameraRotationX;
            _currentCamRotX = Mathf.Clamp(_currentCamRotX, -_camRotLimit, _camRotLimit);
            // Apply rotation to camera transform
            _camera.transform.localEulerAngles = new Vector3(_currentCamRotX, 0f, 0f);
        }
    }

    /// <summary>
    /// Updates movement via user input
    /// </summary>
    internal void UpdateMovement()
    {
        // Calculate movement velocity as a 3D Vector
        float xMov = Input.GetAxisRaw("Horizontal");
        float zMov = Input.GetAxisRaw("Vertical");

        Vector3 mHorizontal = transform.right * xMov;
        Vector3 mVertical = transform.forward * zMov;
        Vector3 velocity = (mHorizontal + mVertical).normalized * SPEED;

        _rb.velocity = velocity;
    }
}
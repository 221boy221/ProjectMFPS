using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour {

    [SerializeField] private Camera _camera;
    private Vector3 _velocity = Vector3.zero;
    private Vector3 _rotation = Vector3.zero;
    private float _cameraRotationX = 0f;
    private float _currentCamRotX = 0f;
    [SerializeField] private float _camRotLimit = 85f;
    private Rigidbody _rb;

    void Start() {
        _rb = GetComponent<Rigidbody>();
    }

    // Gets a movement Vector from PlayerController
    internal void Move(Vector3 velocity) {
        _velocity = velocity;
    }

    // Gets a rotation Vector from PlayerController
    internal void Rotate(Vector3 rotation) {
        _rotation = rotation;
    }
    // Gets a rotation Vector from PlayerController for camera
    internal void RotateCamera(float cameraRotationX) {
        _cameraRotationX = cameraRotationX;
    }
    
    // Run every physics iteration
    void FixedUpdate() {
        PerformMovement();
        PerformRotation();
    }

    // Perform movement based on velocity veriable
    private void PerformMovement() {
        if (_velocity != Vector3.zero) {
            _rb.MovePosition(_rb.position + _velocity * Time.fixedDeltaTime);
        }
    }

    // Perform rotation
    private void PerformRotation() {
        _rb.MoveRotation(_rb.rotation * Quaternion.Euler(_rotation));

        if(_camera != null) {
            // Set rotation and clamp it
            _currentCamRotX -= _cameraRotationX;
            _currentCamRotX = Mathf.Clamp(_currentCamRotX, -_camRotLimit, _camRotLimit);
            // Apply rotation to camera transform
            _camera.transform.localEulerAngles = new Vector3(_currentCamRotX, 0f, 0f);
        }
    }

}

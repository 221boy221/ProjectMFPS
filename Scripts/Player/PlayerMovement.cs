using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour {
    //Movement
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private float _mouseSensitivity = 3f;
    private Vector3 _velocity = Vector3.zero;
    private Vector3 _rotation = Vector3.zero;
    private Rigidbody _rb;
    //Camera
    [SerializeField] private Camera _camera;
    [SerializeField] private float _cameraRotationX = 0f;
    private float _camRotLimit = 85f;
    private float _currentCamRotX = 0f;
    //Networking
    private PhotonView _photonView;
    private Vector3 m_NetworkPosition;
    private Quaternion m_NetworkRotation;
    private float m_MovementSpeed;
    private double m_LastNetworkDataReceivedTime;

    void Start() {
        _rb = GetComponent<Rigidbody>();
        _photonView = GetComponent<PhotonView>();
    }

    internal void SerializeState(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.isWriting) {
            stream.SendNext(_rb.position);
            stream.SendNext(_rb.rotation);
            stream.SendNext(m_MovementSpeed);
        } else {
            m_NetworkPosition = (Vector3)stream.ReceiveNext();
            m_NetworkRotation = (Quaternion)stream.ReceiveNext();
            m_MovementSpeed = (float)stream.ReceiveNext();

            m_LastNetworkDataReceivedTime = info.timestamp;
        }
    }

    void Update() {
        if (_photonView.isMine) {
            CalcPosition();
            CalcRotation();
        } else {
            CalcNetworkedPosition();
            CalcNetworkedRotation();
        }

        UpdatePosition();
        UpdateRotation();
    }

    private void CalcNetworkedPosition() {
        float pingInSeconds = (float)PhotonNetwork.GetPing() * 0.001f;
        float timeSinceLastUpdate = (float)(PhotonNetwork.time - m_LastNetworkDataReceivedTime);
        float totalTimePassed = pingInSeconds + timeSinceLastUpdate;

        Vector3 exterpolatedTargetPosition = m_NetworkPosition + _velocity * totalTimePassed;
        Vector3 newVelocity = (exterpolatedTargetPosition - m_NetworkPosition).normalized * m_MovementSpeed;
        //Vector3 newPosition = Vector3.MoveTowards(_rb.position, exterpolatedTargetPosition, m_Speed * Time.deltaTime);


        if (Vector3.Distance(transform.position, exterpolatedTargetPosition) > 1f) {
            _velocity = newVelocity;
        }
    }

    private void CalcNetworkedRotation() {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, m_NetworkRotation, 180f * Time.deltaTime);
    }

    internal void CalcPosition() {
        // Calculate movement velocity as a 3D Vector
        float xMov = Input.GetAxisRaw("Horizontal");
        float zMov = Input.GetAxisRaw("Vertical");

        Vector3 mHorizontal = transform.right * xMov;
        Vector3 mVertical = transform.forward * zMov;
        Vector3 velocity = (mHorizontal + mVertical).normalized * _movementSpeed;

        _velocity = velocity;
    }

    internal void CalcRotation() {
        // Calc X rotation
        float yRot = Input.GetAxisRaw("Mouse X");
        Vector3 rotation = new Vector3(0f, yRot, 0f) * _mouseSensitivity;
        _rotation = rotation;

        // Calc Y rotation
        float xRot = Input.GetAxisRaw("Mouse Y");
        float cameraRotationX = xRot * _mouseSensitivity;
        _cameraRotationX = cameraRotationX;
    }
    

    // Movement
    private void UpdatePosition() {
        if (_velocity == Vector3.zero)
            return;

        _rb.MovePosition(_rb.position + _velocity * Time.fixedDeltaTime);
    }

    // Rotation
    private void UpdateRotation() {
        // Updates PlayerModel X,Y,Z rotation
        _rb.MoveRotation(_rb.rotation * Quaternion.Euler(_rotation));

        // Updates Camera X rotation
        if (_camera != null) {
            // Set rotation and clamp it
            _currentCamRotX -= _cameraRotationX;
            _currentCamRotX = Mathf.Clamp(_currentCamRotX, -_camRotLimit, _camRotLimit);
            // Apply rotation to camera transform
            _camera.transform.localEulerAngles = new Vector3(_currentCamRotX, 0f, 0f);
        }
    }

}

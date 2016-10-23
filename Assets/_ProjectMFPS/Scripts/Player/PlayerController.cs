using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _mouseSensitivity = 3f;

    private PlayerMotor _motor;

    void Start() {
        _motor = GetComponent<PlayerMotor>();
    }

    void Update() {
        // Calculate movement velocity as a 3D Vector
        float xMov = Input.GetAxisRaw("Horizontal");
        float zMov = Input.GetAxisRaw("Vertical");

        Vector3 mHorizontal = transform.right * xMov;
        Vector3 mVertical = transform.forward * zMov;

        // Final movement vector
        Vector3 velocity = (mHorizontal + mVertical).normalized * _speed;

        // Apply movement
        _motor.Move(velocity);

        // Calculate rotation as a 3D Vector (turning around)
        float yRot = Input.GetAxisRaw("Mouse X");
        Vector3 rotation = new Vector3(0f, yRot, 0f) * _mouseSensitivity;

        // Apply rotation
        _motor.Rotate(rotation);

        // Calculate camera rotation as a 3D Vector
        float xRot = Input.GetAxisRaw("Mouse Y");
        float cameraRotationX = xRot * _mouseSensitivity;

        // Apply rotation
        _motor.RotateCamera(cameraRotationX);
    }

}

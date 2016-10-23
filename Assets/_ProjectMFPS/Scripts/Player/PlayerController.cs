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
        Vector3 velocity = (mHorizontal + mVertical).normalized * _speed;

        _motor.Move(velocity);


        // Calc X rotation
        float yRot = Input.GetAxisRaw("Mouse X");
        Vector3 rotation = new Vector3(0f, yRot, 0f) * _mouseSensitivity;
        
        _motor.Rotate(rotation);


        // Calc Y rotation
        float xRot = Input.GetAxisRaw("Mouse Y");
        float cameraRotationX = xRot * _mouseSensitivity;
        
        _motor.RotateCamera(cameraRotationX);
    }

}

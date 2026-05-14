using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PlayerCamera : MonoBehaviour {
    public float mouseSensitivity = 25f;
    public Transform playerBody;
    public PlayerState playerState;
    public Camera thisCamera;

    private float xRotation = 0f;

    void OnDrawGizmosSelected() {
        float angle = GetCameraViewAngle();

        Vector3 leftBoundary = Quaternion.Euler(0, -angle, 0) * thisCamera.transform.forward * 20f;
        Vector3 rightBoundary = Quaternion.Euler(0, angle, 0) * thisCamera.transform.forward * 20f;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(thisCamera.transform.position, thisCamera.transform.position + leftBoundary);
        Gizmos.DrawLine(thisCamera.transform.position, thisCamera.transform.position + rightBoundary);
        Gizmos.DrawWireSphere(thisCamera.transform.position, 20f);
    }

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }

    void Update() {
        if(Cursor.lockState != CursorLockMode.Locked) return;
        if (playerState.isDead) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    public bool IsInViewAngleOfPlayer(Vector3 position, float angleBuffer = 0f) {
        Vector3 direction = (position - thisCamera.transform.position).normalized;
        float currentViewAngle = Vector3.Angle(direction, Camera.main.transform.forward);

        if (currentViewAngle > GetCameraViewAngle(angleBuffer)) return false;
        return true;
    }

    public float GetCameraViewAngle(float angleBuffer = 0f) {
        float verticalAngle = thisCamera.fieldOfView;
        float horizontalAngle = 0f; // 2f * Mathf.Atan(Mathf.Tan(Camera.main.fieldOfView * Mathf.Deg2Rad / 2f) * Camera.main.aspect) * Mathf.Rad2Deg;

        return Mathf.Max(verticalAngle, horizontalAngle) + angleBuffer;
    }
}

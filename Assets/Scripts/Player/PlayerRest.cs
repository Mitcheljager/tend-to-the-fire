using UnityEngine;

public class PlayerRest : MonoBehaviour {
    [Fade] public bool isResting = false;

    private PlayerCamera playerCamera;

    void Awake() {
        playerCamera = FindFirstObjectByType<PlayerCamera>();
    }

    void Update() {
        if (!isResting) return;

        float movementInput = Input.GetAxisRaw("Horizontal") + Input.GetAxisRaw("Vertical");

        if (movementInput != 0f) SetResting(false, null);
    }

    public void SetResting(bool state, Transform restCameraPosition) {
        isResting = state;

        if (!state) return;

        if (playerCamera == null) playerCamera = FindFirstObjectByType<PlayerCamera>();

        playerCamera.playerBody.transform.position = restCameraPosition.position;
        playerCamera.SetCameraLimitAngle(restCameraPosition.eulerAngles.y);
        playerCamera.SetCameraFacingDirection(restCameraPosition.transform.forward);
    }
}

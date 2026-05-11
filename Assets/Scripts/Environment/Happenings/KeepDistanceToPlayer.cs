using UnityEngine;

public class KeepDistanceToPlayer : MonoBehaviour {
    public float distance = 5f;
    public Axis onAxis = Axis.X;

    public enum Axis { X, Y, Z }

    private PlayerState playerState;

    void Start() {
        playerState = FindFirstObjectByType<PlayerState>();
    }

    void LateUpdate() {
        Vector3 playerPosition = playerState.transform.position;
        Vector3 objectPosition = transform.position;

        if (onAxis == Axis.X) {
            objectPosition.x = playerPosition.x + (playerState.transform.forward.x * distance);
        } else if (onAxis == Axis.Y) {
            objectPosition.y = playerPosition.y + (playerState.transform.forward.y * distance);
        } else if (onAxis == Axis.Z) {
            objectPosition.z = playerPosition.z + (playerState.transform.forward.z * distance);
        }

        transform.position = objectPosition;
    }
}

using UnityEngine;

[RequireComponent(typeof(BoxColliderGizmos))]
public class AttachToPlayerOnTriggerStay : MonoBehaviour {
    [Header("Objects")]
    public GameObject objectToAttach;

    private PlayerState playerState;

    void OnDrawGizmos() {
        if (this.enabled) Gizmos.DrawIcon(transform.position, "attach.png", false);
    }

    void OnTriggerStay() {
        objectToAttach.transform.position = playerState.transform.position;
    }

    void Start() {
        playerState = FindFirstObjectByType<PlayerState>();
    }
}

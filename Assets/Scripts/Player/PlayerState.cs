using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerState : MonoBehaviour {
    [Header("State")]
    public bool isDead = false;
    [Header("Animation")]
    public Transform rightHandTransform;

    private Rigidbody playerRigidbody;

    void Start () {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    public void Kill() {
        isDead = true;
        playerRigidbody.isKinematic = true;

        PlayerDeathEvent.Dispatch();
    }
}

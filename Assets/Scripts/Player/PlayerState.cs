using UnityEngine;

public class PlayerState : MonoBehaviour {
    [Header("State")]
    public bool isDead = false;
    [Header("Animation")]
    public Transform rightHandTransform;

    public void Kill() {
        isDead = true;
    }
}

using UnityEngine;

public class PlayerState : MonoBehaviour {
    [Header("State")]
    public bool isDead = false;

    public void Kill() {
        isDead = true;
    }
}

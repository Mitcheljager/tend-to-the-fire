using UnityEngine;

public class SlowPlayerWhileInside : MonoBehaviour {
    [Header("State")]
    [Fade] public bool isPlayerInside = false;

    void OnTriggerEnter(Collider collider) {
        if (!collider.CompareTag("Player")) return;

        isPlayerInside = true;
    }

    void OnTriggerExit(Collider collider) {
        if (!collider.CompareTag("Player")) return;

        isPlayerInside = false;
    }
}

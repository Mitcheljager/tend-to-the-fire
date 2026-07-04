using System.Collections;
using UnityEngine;

public class EnemyKillRadius : MonoBehaviour {
    PlayerState playerState;

    void Start() {
        playerState = FindFirstObjectByType<PlayerState>();
    }

    void OnTriggerEnter(Collider collider) {
        if (!collider.CompareTag("Player")) return;

        playerState.Kill();
    }
}

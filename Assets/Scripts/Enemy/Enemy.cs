using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public float killRadius;
    public AudioHelper audioHelperFocus;

    private EnemyManager enemyManager;

    public void Start() {
        enemyManager = FindFirstObjectByType<EnemyManager>();

        SetFocusAudioProgress();
    }

    public void Despawn() {
        if (enemyManager != null) enemyManager.DespawnEnemy(this);
    }

    private void SetFocusAudioProgress() {
        if (enemyManager != null) audioHelperFocus.audioSource.time = enemyManager.GetFocusAudioProgress();
    }
}

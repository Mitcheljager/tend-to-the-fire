using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public float killRadius;
    public AudioHelper audioHelperFocus;
    public EventWhenOutOfView eventWhenOutOfView;

    private EnemyManager enemyManager;

    public void Start() {
        enemyManager = FindFirstObjectByType<EnemyManager>();

        SetFocusAudioProgress();
    }

    public void Despawn() {
        if (enemyManager != null) enemyManager.DespawnEnemy(this);
    }

    public bool IsInView() {
        return eventWhenOutOfView.IsInView();
    }

    private void SetFocusAudioProgress() {
        if (enemyManager != null) audioHelperFocus.audioSource.time = enemyManager.GetFocusAudioProgress();
    }
}

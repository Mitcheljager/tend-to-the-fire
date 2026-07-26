using UnityEngine;

public class Enemy : MonoBehaviour {
    public AudioHelper audioHelperFocus;
    public EventWhenOutOfView eventWhenOutOfView;

    private EnemyManager enemyManager;
    private PlayerState playerState;

    public void Start() {
        enemyManager = FindFirstObjectByType<EnemyManager>();
        playerState = FindFirstObjectByType<PlayerState>();

        SetFocusAudioProgress();
    }

    public void Despawn() {
        if (enemyManager != null) enemyManager.DespawnEnemy(this);
    }

    public void Reposition() {
        if (enemyManager != null) enemyManager.RepositionEnemy(this);
    }

    public void DespawnWhenPlayerIsInTotalSafetyRange() {
        if (playerState.isInTotalSafetyRange) Despawn();
    }

    public bool IsInView() {
        return eventWhenOutOfView.IsInView();
    }

    private void SetFocusAudioProgress() {
        if (enemyManager != null) audioHelperFocus.audioSource.time = enemyManager.GetFocusAudioProgress();
    }
}

using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public float killRadius;

    private EnemyManager enemyManager;

    public void Start() {
        enemyManager = FindFirstObjectByType<EnemyManager>();
    }

    public void Despawn() {
        if (enemyManager != null) enemyManager.DespawnEnemy(this);
    }
}

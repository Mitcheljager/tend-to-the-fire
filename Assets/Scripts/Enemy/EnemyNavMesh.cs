using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Enemy))]
public class EnemyNavMesh : MonoBehaviour {
    public NavMeshAgent navMeshAgent;

    private EnemyManager enemyManager;

    void Start() {
        enemyManager = FindFirstObjectByType<EnemyManager>();
    }

    public void SetDestination(Vector3 position) {
        navMeshAgent.SetDestination(position);
    }
}

using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Enemy))]
public class EnemyNavigation : MonoBehaviour {
    public NavMeshAgent agent;
    [Header("State")]
    [Fade] public bool isFollowingPlayer;
    [Fade] public bool isStopped;

    private EnemyManager enemyManager;
    private PlayerState playerState;

    void Start() {
        enemyManager = FindFirstObjectByType<EnemyManager>();
        playerState = FindFirstObjectByType<PlayerState>();
    }

    void Update() {
        isStopped = agent.isStopped;

        if (isFollowingPlayer) return;
        if (agent.isStopped) return;

        if (isFollowingPlayer) {
            SetDestination(playerState.transform.position);
            return;
        }

        if (agent.remainingDistance > agent.stoppingDistance) return;

        SetRandomValidDestination();
    }

    public void SetDestination(Vector3 position) {
        agent.SetDestination(position);
    }

    public void StartFollowingPlayer() {
        if (playerState.isDead) return;

        isFollowingPlayer = true;

        SetDestination(playerState.transform.position);
    }

    public void EndFollowingPlayer() {
        isFollowingPlayer = false;

        SetRandomValidDestination();
    }

    private void SetRandomValidDestination() {
        SetDestination(enemyManager.FindRandomPositionOutsideOfFire());
    }
}

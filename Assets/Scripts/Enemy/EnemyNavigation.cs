using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Enemy))]
public class EnemyNavigation : MonoBehaviour {
    public NavMeshAgent agent;
    [Header("Config")]
    public float baseSpeed = 2f;
    public float runSpeed = 5f;
    [Header("State")]
    [Fade] public bool isFollowingPlayer;
    [Fade] public bool isStopped;

    private Enemy enemy;
    private EnemyManager enemyManager;
    private PlayerState playerState;
    private Fire fire;

    void Start() {
        enemy = FindFirstObjectByType<Enemy>();
        enemyManager = FindFirstObjectByType<EnemyManager>();
        playerState = FindFirstObjectByType<PlayerState>();
        fire = FindFirstObjectByType<Fire>();
    }

    void Update() {
        isStopped = agent.isStopped;

        SetSpeed();

        if (isFollowingPlayer) return;
        if (agent.isStopped) return;

        if (Vector3.Distance(transform.position, fire.transform.position) < fire.currentTotalSafetyRange) {
            enemy.Despawn();
            return;
        }

        if (isFollowingPlayer) {
            SetDestination(playerState.transform.position);
            return;
        }

        if (agent.remainingDistance < agent.stoppingDistance) {
            SetRandomValidDestination();
        }
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

    public void SetRandomValidDestination() {
        SetDestination(enemyManager.FindRandomPositionOutsideOfFire());
    }

    private void SetSpeed() {
        agent.speed = isFollowingPlayer ? runSpeed : baseSpeed;
    }
}

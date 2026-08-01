using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Enemy))]
public class EnemyNavigation : MonoBehaviour {
    public NavMeshAgent agent;
    [Header("Speed")]
    public float baseSpeed = 2f;
    public float runSpeed = 5f;
    public float speedUpMaximumDistance = 100f;
    public AnimationCurve distanceSpeedMultiplierCurve = new(new Keyframe(0f, 1f), new Keyframe(0.75f, 1f), new Keyframe(1f, 0f));
    [Header("Fire distance")]
    public float resetDestinationPastDistanceToPlayer = 80f;
    public float fireDistanceDestinationGuideMaximum = 30f;
    public AnimationCurve fireDistanceDestinationGuideCurve = new(new Keyframe(0f, 1f), new Keyframe(0.75f, 1f), new Keyframe(1f, 0f));
    [Header("State")]
    [Fade] public bool isFollowingPlayer;

    private Enemy enemy;
    private EnemyManager enemyManager;
    private PlayerState playerState;
    private Fire fire;

    void OnDrawGizmos() {
        if (playerState == null) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(playerState.transform.position, GetGuideRange() * 0.5f);
        Gizmos.DrawWireSphere(playerState.transform.position, GetGuideRange());

        #if UNITY_EDITOR
            if (agent.destination.y != float.PositiveInfinity) UnityEditor.Handles.Label(agent.destination + Vector3.up * 1.5f, "Destination");
        #endif
    }

    void Start() {
        enemy = FindFirstObjectByType<Enemy>();
        enemyManager = FindFirstObjectByType<EnemyManager>();
        playerState = FindFirstObjectByType<PlayerState>();
        fire = FindFirstObjectByType<Fire>();
    }

    void Update() {
        SetSpeed();

        if (isFollowingPlayer) return;
        if (agent.isStopped) return;
        if (agent.pathPending) return;

        if (Vector3.Distance(transform.position, fire.transform.position) < fire.currentTotalSafetyRange) {
            enemy.Despawn();
            return;
        }

        if (isFollowingPlayer) {
            SetDestination(playerState.transform.position);
            return;
        }

        if (Vector3.Distance(agent.destination, playerState.transform.position) > resetDestinationPastDistanceToPlayer) {
            SetRandomValidDestination();
            return;
        }

        if (agent.remainingDistance <= agent.stoppingDistance) {
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

    public float GetGuideRange() {
        float playerDistanceFromTotalSafety = Vector3.Distance(playerState.transform.position, fire.transform.position) - fire.currentTotalSafetyRange;
        float normalizedDistance = 1f / fireDistanceDestinationGuideMaximum * playerDistanceFromTotalSafety;

        return fireDistanceDestinationGuideMaximum * fireDistanceDestinationGuideCurve.Evaluate(normalizedDistance);
    }

    public void SetRandomValidDestination() {
        float maxRange = GetGuideRange();

        SetDestination(enemyManager.FindValidPositionAroundPlayer(maxRange * 0.5f, maxRange));
    }

    private void SetSpeed() {
        float distanceFromPlayer = Vector3.Distance(playerState.transform.position, transform.position);
        float normalizedDistance = 1f / speedUpMaximumDistance * distanceFromPlayer;

        agent.speed = (isFollowingPlayer ? runSpeed : baseSpeed) * distanceSpeedMultiplierCurve.Evaluate(normalizedDistance);;
    }
}

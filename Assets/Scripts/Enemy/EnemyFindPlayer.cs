using System.Collections;
using UnityEngine;

public class EnemyFindPlayer : MonoBehaviour {
    [Header("Config")]
    public LayerMask layerMask;
    public float range = 20f;
    public float maxAngle = 80f;
    public float autoDetectInRadius = 3f;
    public float secondsToDetectPlayer = 1f;

    private Enemy enemy;
    private EnemyNavigation enemyNavigation;
    private EnemyManager enemyManager;
    private PlayerState playerState;

    void OnDrawGizmosSelected() {
        Vector3 leftBoundary = Quaternion.Euler(0, -maxAngle / 2, 0) * transform.forward * range;
        Vector3 rightBoundary = Quaternion.Euler(0, maxAngle / 2, 0) * transform.forward * range;

        Gizmos.color = Color.purple;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
        Gizmos.DrawWireSphere(transform.position, range);
        Gizmos.DrawWireSphere(transform.position, autoDetectInRadius);

        if (playerState == null) return;

        Gizmos.color = IsPlayerSeen() ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position, playerState.transform.position);

        UnityEditor.Handles.Label(transform.position + Vector3.up * 1.5f, "Seen: " + IsPlayerSeen());
    }

    void Start() {
        enemy = GetComponent<Enemy>();
        enemyNavigation = GetComponent<EnemyNavigation>();
        enemyManager = FindFirstObjectByType<EnemyManager>();
        playerState = FindFirstObjectByType<PlayerState>();
    }

    void Update() {
        if (playerState.isDead) return;
        if (!IsPlayerSeen()) return;

        StartLookingForPlayer();
    }

    public bool IsPlayerSeen() {
        Vector3 playerPosition = playerState.transform.position;
        Vector3 direction = (playerPosition - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, playerState.transform.position);

        if (!Physics.SphereCast(transform.position, 0.05f, direction, out RaycastHit hit, range, layerMask)) return false;
        if (!hit.collider.CompareTag("Player")) return false;
        if (distance < autoDetectInRadius) return true;
        if (!enemyNavigation.isFollowingPlayer && !IsInViewAngle(playerPosition, transform.position, transform.forward)) return false;

        return true;
    }

    private bool IsInViewAngle(Vector3 from, Vector3 to, Vector3 directionTo) {
        Vector3 direction = (from - to).normalized;
        float currentAngle = Vector3.Angle(direction, directionTo);

        if (currentAngle > maxAngle / 2) return false;
        return true;
    }

    public void StartLookingForPlayer() {
        if (enemyNavigation.agent.isStopped) return;

        StartCoroutine(PossiblyDelayDetectPlayer());
    }

    private IEnumerator PossiblyDelayDetectPlayer() {
        if (!enemyNavigation.isFollowingPlayer) enemyNavigation.agent.isStopped = true;

        float seenTime = 0f;
        float checkInterval = 0.1f;
        float elapsedTime = 0f;

        while (elapsedTime < secondsToDetectPlayer) {
            yield return new WaitForSeconds(checkInterval);
            elapsedTime += checkInterval;

            if (IsPlayerSeen()) seenTime += checkInterval;
        }

        if (IsPlayerSeen()) {
            // If player is seen for the full duration start following the player
            enemyNavigation.StartFollowingPlayer();
        } else if (seenTime >= secondsToDetectPlayer / 2) {
            // If player is seen for at least half of the detection time set destination without actually following the player
            enemyNavigation.SetDestination(playerState.transform.position);
        } else {
            // Reset if seen for less than half the time
            enemyNavigation.EndFollowingPlayer();
        }

        enemyNavigation.agent.isStopped = false;
    }
}

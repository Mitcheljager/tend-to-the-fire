using UnityEngine;

public class PlayerEnemyAttraction : MonoBehaviour {
    [Header("Config")]
    public float maxDistance = 5f;
    public float lerpSpeedBase = 10f;
    public AnimationCurve lerpSpeedCurve = new(new Keyframe(0f, 1f), new Keyframe(0.75f, 1f), new Keyframe(1f, 0f));
    [Header("State")]
    [Fade] public Enemy nearestEnemy;

    private EnemyManager enemyManager;
    private PlayerCamera playerCamera;

    void Start() {
        enemyManager = FindFirstObjectByType<EnemyManager>();
        playerCamera = FindFirstObjectByType<PlayerCamera>();
    }

    void LateUpdate() {
        nearestEnemy = enemyManager.FindNearestEnemyToPosition(transform.position);

        if (nearestEnemy != null) RotateTowardsNearestEnemy();
    }

    private void RotateTowardsNearestEnemy() {
        float distance = Vector3.Distance(transform.position, nearestEnemy.transform.position);

        if (distance > maxDistance) return;

        float lerpSpeed = Time.deltaTime * lerpSpeedBase * lerpSpeedCurve.Evaluate(1f / maxDistance * distance);

        Vector3 target = nearestEnemy.transform.position;
        Vector3 direction = (target - playerCamera.playerBody.position).normalized;
        Vector3 targetDirection = Vector3.Lerp(playerCamera.transform.forward, direction, lerpSpeed);

        playerCamera.SetCameraFacingDirection(targetDirection);
    }
}

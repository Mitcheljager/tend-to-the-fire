using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour {
    [Header("Config")]
    public float maxRadius = 10f;
    public float ignoreViewAngleFromDistance = 20f;
    public float spawnDistanceFromFloor = 1f;
    public int maxNumberOfEnemies = 500;
    [Header("Objects")]
    public GameObject enemyPrefab;
    [Header("State")]
    public List<Enemy> enemies;

    private Fire fire;
    private PlayerState playerState;
    private PlayerCamera playerCamera;
    private PlayerFocus playerFocus;

    void OnDrawGizmos() {
        if (playerCamera == null) return;

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(playerCamera.transform.position, maxRadius);
    }

    private void OnEnable() {
        PlayerEvent.OnPlayerEnteredTotalSafetyRange.AddListener(DespawnAllOutOfViewEnemies);
    }

    private void OnDisable() {
        PlayerEvent.OnPlayerEnteredTotalSafetyRange.RemoveListener(DespawnAllOutOfViewEnemies);
    }

    void Start() {
        fire = FindFirstObjectByType<Fire>();
        playerState = FindFirstObjectByType<PlayerState>();
        playerCamera = FindFirstObjectByType<PlayerCamera>();
        playerFocus = FindFirstObjectByType<PlayerFocus>();

        StartCoroutine(RepeatedlySpawnEnemies());
    }

    private void SpawnEnemy() {
        Vector3? position = FindValidPosition();

        if (position == null) return;

        GameObject instantiatedEnemy = Instantiate(enemyPrefab, position.Value, transform.rotation);
        instantiatedEnemy.transform.parent = transform;
        instantiatedEnemy.name = instantiatedEnemy.name + " " + enemies.Count;

        Enemy enemy = instantiatedEnemy.GetComponent<Enemy>();
        enemies.Add(enemy);
    }

    public void DespawnEnemy(Enemy enemy) {
        enemies.Remove(enemy);
        Destroy(enemy.gameObject);
    }

    public void RepositionEnemy(Enemy enemy) {
        if (!this.enabled) return;

        Vector3? position = FindValidPosition();

        if (position == null) {
            DespawnEnemy(enemy);
            return;
        }

        enemy.transform.position = position.Value;
    }

    public void DespawnAllOutOfViewEnemies() {
        foreach(Enemy enemy in enemies.ToList()) {
            if (!enemy.IsInView()) DespawnEnemy(enemy);
        }
    }

    public void DespawnAllEnemies() {
        foreach(Enemy enemy in enemies.ToList()) {
            DespawnEnemy(enemy);
        }
    }

    public Vector3 FindRandomPositionOutsideOfFire() {
        return FindRandomPointAlongRadius(playerCamera.transform.position, maxRadius * 0.75f, maxRadius);
    }

    // https://discussions.unity.com/t/random-point-within-circle-with-min-max-radius/724904/14
    public Vector3 FindRandomPointAlongRadius(Vector3 origin, float minRadius, float maxRadius) {
        Vector2 direction = Random.insideUnitCircle.normalized;
        float distance = Random.Range(minRadius, maxRadius);

        return origin + new Vector3(direction.x, 0, direction.y) * distance;
    }

    public float GetFocusAudioProgress() {
        if (enemies.Count == 0) return 0;
        if (enemies[0] == null) return 0;

        return enemies[0].audioHelperFocus.audioSource.time;
    }

    public Enemy FindNearestEnemyToPosition(Vector3 position) {
        if (enemies.Count == 0) return null;

        Enemy nearestEnemy = null;
        float nearestDistance = 1000f;

        foreach(Enemy enemy in enemies.ToList()) {
            float distance = Vector3.Distance(enemy.transform.position, position);

            if (distance < nearestDistance) {
                nearestEnemy = enemy;
                nearestDistance = distance;
            }
        }

        return nearestEnemy;
    }

    private Vector3? FindValidPosition() {
        Vector3 position = FindRandomPositionOutsideOfFire();

        int safety = 0;

        if (Vector3.Distance(position, fire.transform.position) < ignoreViewAngleFromDistance) {
            while (Vector3.Distance(position, fire.transform.position) < fire.currentLightRange || (!playerFocus.isFullyClosed && playerCamera.IsInViewAngleOfPlayer(position))) {
                position = FindRandomPositionOutsideOfFire();
                safety++;

                if (safety > 100) return null;
            }
        }

        Vector3 abovePosition = position + Vector3.up * 20f;
        if (!Physics.Raycast(abovePosition, Vector3.down, out RaycastHit floorHit)) return null;

        return floorHit.point + Vector3.up * spawnDistanceFromFloor;
    }

    private IEnumerator RepeatedlySpawnEnemies() {
        while (enemies.Count < maxNumberOfEnemies) {
            yield return new WaitForSeconds(0.1f);

            if (!playerState.isInTotalSafetyRange) SpawnEnemy();
        }
    }
}

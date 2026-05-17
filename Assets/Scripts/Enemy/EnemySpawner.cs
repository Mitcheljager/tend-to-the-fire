using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    public float maxRadius = 10f;
    public float ignoreViewAngleFromDistance = 20f;
    public GameObject enemyPrefab;
    public Fire fire;

    private PlayerCamera playerCamera;
    private PlayerFocus playerFocus;

    void OnDrawGizmos() {
        if (playerCamera == null) return;

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(playerCamera.transform.position, maxRadius);
    }

    void Start() {
        playerCamera = FindFirstObjectByType<PlayerCamera>();
        playerFocus = FindFirstObjectByType<PlayerFocus>();

        StartCoroutine(RepeatedlySpawnEnemies());
    }

    private void SpawnEnemy() {
        Vector3 position = FindRandomPositionOutsideOfFire();

        int safety = 0;

        if (Vector3.Distance(position, fire.transform.position) < ignoreViewAngleFromDistance) {
            while (Vector3.Distance(position, fire.transform.position) < fire.currentLightRange || (!playerFocus.isFullyClosed && playerCamera.IsInViewAngleOfPlayer(position))) {
                position = FindRandomPositionOutsideOfFire();
                safety++;

                if (safety > 100) return;
            }
        }

        Vector3 abovePosition = position + Vector3.up * 20f;
        if (!Physics.Raycast(abovePosition, Vector3.down, out RaycastHit floorHit)) return;

        GameObject enemy = Instantiate(enemyPrefab, floorHit.point, transform.rotation);
        enemy.transform.parent = transform;
    }

    public Vector3 FindRandomPositionOutsideOfFire() {
        return FindRandomPointAlongRadius(playerCamera.transform.position, maxRadius * 0.75f, maxRadius);
    }

    // https://discussions.unity.com/t/random-point-within-circle-with-min-max-radius/724904/14
    public Vector3 FindRandomPointAlongRadius(Vector3 origin, float minRadius, float maxRadius){
        Vector2 direction = Random.insideUnitCircle.normalized;
        float distance = Random.Range(minRadius, maxRadius);

        return origin + new Vector3(direction.x, 0, direction.y) * distance;
    }

    private IEnumerator RepeatedlySpawnEnemies() {
        for (int i = 0; i < 500; i++) {
            yield return new WaitForSeconds(0.1f);

            SpawnEnemy();
        }
    }
}

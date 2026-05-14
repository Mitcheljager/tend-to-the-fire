using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    public float maxRadius = 10f;
    public GameObject enemyPrefab;
    public Fire campfire;

    private PlayerState playerState;
    private PlayerCamera playerCamera;

    void OnDrawGizmos() {
        if (playerState == null) return;

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(playerState.transform.position, maxRadius);
    }

    void Start() {
        playerState = FindFirstObjectByType<PlayerState>();
        playerCamera = FindFirstObjectByType<PlayerCamera>();

        SpawnEnemies();
    }

    private void SpawnEnemies() {
        for (int i = 0; i < 500; i++) {
            Vector3 position = FindRandomPositionOutsideOfCampfire();

            int safety = 0;
            while (Vector3.Distance(position, campfire.transform.position) < campfire.currentLightRange || playerCamera.IsInViewAngleOfPlayer(position)) {
                position = FindRandomPositionOutsideOfCampfire();
                safety++;

                if (safety > 100) return;
            }

            Instantiate(enemyPrefab, position, transform.rotation);
        }
    }

    public Vector3 FindRandomPositionOutsideOfCampfire() {
        return FindRandomPointAlongRadius(playerState.transform.position, maxRadius * 0.75f, maxRadius);
    }

    // https://discussions.unity.com/t/random-point-within-circle-with-min-max-radius/724904/14
    public Vector3 FindRandomPointAlongRadius(Vector3 origin, float minRadius, float maxRadius){
        Vector2 direction = Random.insideUnitCircle.normalized;
        float distance = Random.Range(minRadius, maxRadius);

        return origin + new Vector3(direction.x, 0, direction.y) * distance;
    }
}

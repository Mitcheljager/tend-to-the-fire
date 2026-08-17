using UnityEngine;

public class GameManager : MonoBehaviour {
    [Header("Components")]
    public EnemyManager enemyManager;
    public Fire fire;
    [Header("State")]
    public bool hasGameStarted = false;

    private int initialMaxNumberOfEnemies = 0;
    private float initialFuelConsumptionPerSecond = 0f;

    void Awake() {
        initialMaxNumberOfEnemies = enemyManager.maxNumberOfEnemies;
        enemyManager.maxNumberOfEnemies = 0;
        enemyManager.DespawnAllEnemies();

        initialFuelConsumptionPerSecond = fire.fuelConsumptionPerSecond;
        fire.fuelConsumptionPerSecond = 0f;
    }

    public void StartGame() {
        hasGameStarted = true;

        enemyManager.maxNumberOfEnemies = initialMaxNumberOfEnemies;
        fire.fuelConsumptionPerSecond = initialFuelConsumptionPerSecond;
    }
}

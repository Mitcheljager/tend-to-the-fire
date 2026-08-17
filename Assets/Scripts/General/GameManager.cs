using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour {
    [Header("Components")]
    public EnemyManager enemyManager;
    public Fire fire;
    [Header("Config")]
    public float endTimeSeconds = 60000f;
    public UnityEvent onGameEnd;
    [Header("State")]
    [Fade] public bool hasGameStarted = false;
    [Fade] public bool hasGameEnded = false;
    [Fade] public float currentTimeSeconds = 0f;

    private int initialMaxNumberOfEnemies = 0;
    private float initialFuelConsumptionPerSecond = 0f;

    void Awake() {
        initialMaxNumberOfEnemies = enemyManager.maxNumberOfEnemies;
        enemyManager.maxNumberOfEnemies = 0;
        enemyManager.DespawnAllEnemies();

        initialFuelConsumptionPerSecond = fire.fuelConsumptionPerSecond;
        fire.fuelConsumptionPerSecond = 0f;
    }

    void Update() {
        if (!hasGameStarted) return;

        currentTimeSeconds += Time.deltaTime;

        if (currentTimeSeconds >= endTimeSeconds) EndGame();
    }

    public void StartGame() {
        hasGameStarted = true;

        enemyManager.maxNumberOfEnemies = initialMaxNumberOfEnemies;
        fire.fuelConsumptionPerSecond = initialFuelConsumptionPerSecond;
    }

    public void EndGame() {
        hasGameEnded = true;

        onGameEnd.Invoke();
    }
}

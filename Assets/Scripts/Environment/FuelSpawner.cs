using UnityEngine;

public class FuelSpawner : MonoBehaviour {
    public GameObject[] fuelPrefabs;
    [Header("State")]
    public bool hasSpawnedFuel = false;

    private FuelSpawnersManager fuelSpawnersManager;

    void OnDrawGizmos() {
        Gizmos.DrawIcon(transform.position, "activity.png", true, Color.cyan);
    }

    public void Spawn() {
        if (!fuelSpawnersManager) fuelSpawnersManager = FindAnyObjectByType<FuelSpawnersManager>();

        GameObject prefab = fuelPrefabs[Random.Range(0, fuelPrefabs.Length)];
        Quaternion randomRotation = Quaternion.Euler(0f, Random.Range(0, 360f), 0f);

        GameObject instantiatedObject = Instantiate(prefab, transform.position, randomRotation);
        instantiatedObject.transform.parent = transform;

        Fuel fuel = instantiatedObject.GetComponent<Fuel>();
        fuelSpawnersManager.AddFuel(fuel);

        hasSpawnedFuel = true;
    }
}

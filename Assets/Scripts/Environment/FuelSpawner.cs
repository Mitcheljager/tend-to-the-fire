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

        if (!Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hit, 5f)) return;

        Vector3 position = hit.point;
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        Quaternion randomRotation = Quaternion.Euler(0f, Random.Range(0, 360f), 0f);

        GameObject prefab = fuelPrefabs[Random.Range(0, fuelPrefabs.Length)];
        GameObject instantiatedObject = Instantiate(prefab, position, rotation * randomRotation);
        instantiatedObject.transform.parent = transform;

        Fuel fuel = instantiatedObject.GetComponent<Fuel>();
        fuelSpawnersManager.AddFuel(fuel);

        hasSpawnedFuel = true;
    }
}

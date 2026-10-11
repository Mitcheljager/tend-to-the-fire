using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FuelSpawnersManager : MonoBehaviour {
    public int numberOfFuelSpawned = 10;
    public int staggerSpawnPerCount = 10;
    [Header("State")]
    public List<Fuel> spawnedFuel;
    public List<FuelSpawner> fuelSpawners;

    void Start() {
        spawnedFuel = FindObjectsByType<Fuel>(FindObjectsSortMode.None).ToList();
        fuelSpawners = FindObjectsByType<FuelSpawner>(FindObjectsSortMode.None).ToList();

        StartCoroutine(SpawnFuel());
    }

    public void AddFuel(Fuel fuel) {
        spawnedFuel.Add(fuel);
    }

    private List<FuelSpawner> GetRandomizedSpawners() {
        System.Random rng = new();

        return fuelSpawners.OrderBy(x => rng.Next()).ToList();
    }

    private IEnumerator SpawnFuel() {
        List<FuelSpawner> randomizedSpawners = GetRandomizedSpawners();

        if (randomizedSpawners.Count == 0) yield return null;

        for (int i = 0; i < Mathf.Min(numberOfFuelSpawned, fuelSpawners.Count); i++) {
            randomizedSpawners[i].Spawn();

            if (i % staggerSpawnPerCount == 0) yield return new WaitForEndOfFrame();
        }
    }
}

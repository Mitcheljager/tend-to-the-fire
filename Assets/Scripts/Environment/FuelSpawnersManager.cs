using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FuelSpawnersManager : MonoBehaviour {
    public int numberOfFuelSpawned = 10;
    [Header("State")]
    public List<Fuel> spawnedFuel;
    public List<FuelSpawner> fuelSpawners;

    void Start() {
        spawnedFuel = FindObjectsByType<Fuel>(FindObjectsSortMode.None).ToList();
        fuelSpawners = FindObjectsByType<FuelSpawner>(FindObjectsSortMode.None).ToList();

        SpawnFuel();
    }

    public void AddFuel(Fuel fuel) {
        spawnedFuel.Add(fuel);
    }

    private List<FuelSpawner> GetRandomizedSpawners() {
        System.Random rng = new();

        return fuelSpawners.OrderBy(x => rng.Next()).ToList();
    }

    private void SpawnFuel() {
        List<FuelSpawner> randomizedSpawners = GetRandomizedSpawners();

        if (randomizedSpawners.Count == 0) return;

        for (int i = 0; i < Mathf.Min(numberOfFuelSpawned, fuelSpawners.Count); i++) {
            randomizedSpawners[i].Spawn();
        }
    }
}

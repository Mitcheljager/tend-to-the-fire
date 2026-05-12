using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fire : Interactable {
    public float maxFuel = 10f;
    public float fuelConsumptionPerSecond = 1f;
    [Header("State")]
    [Fade] public List<Fuel> activeFuel;
    [Fade] public float currentFuel = 0f;

    void Update() {
        DecreaseActiveFuel();
        currentFuel = GetCurrentFuel();
    }

    private float GetCurrentFuel() {
        float total = 0f;

        foreach (Fuel fuel in activeFuel) {
            total += fuel.currentFuel;
        }

        return total;
    }

    private void DecreaseActiveFuel() {
        foreach (Fuel fuel in activeFuel) {
            fuel.DecreaseCurrentFuel(Time.deltaTime * fuelConsumptionPerSecond);
        }

        activeFuel = activeFuel.Where(fuel => fuel.currentFuel > 0).ToList();
    }
}

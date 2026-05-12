using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fire : Interactable {
    public float maxFuel = 10f;
    public float fuelConsumptionPerSecond = 1f;
    [Header("State")]
    [Fade] public List<Fuel> activeFuel;
    [Fade] public float currentFuel = 0f;

    private PlayerInventory playerInventory;

    void Start() {
        playerInventory = FindFirstObjectByType<PlayerInventory>();
    }

    void Update() {
        DecreaseActiveFuel();
        currentFuel = GetCurrentFuel();
    }

    public override void Interact() {
        AddFuelFromPlayerInventory();
    }

    public override string GetInteractableText() {
        return playerInventory.IsCarryingAnyFuel() ? "Tend to the Fire" : "You have nothing left";
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

    private void AddFuelFromPlayerInventory() {
        if (playerInventory.IsCarryingAnyFuel()) return;
    }
}

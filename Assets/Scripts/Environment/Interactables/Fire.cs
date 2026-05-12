using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fire : Interactable {
    [Header("Config")]
    public float maxFuel = 10f;
    public float fuelConsumptionPerSecond = 1f;
    [Header("Text")]
    public string interactTextAble = "Tend to the fire";
    public string interactTextUnable = "You have nothing left";
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
        return playerInventory.IsCarryingAnyFuel() ? interactTextAble : interactTextUnable;
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

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Fire : Interactable {
    [Separator]
    [Header("Config")]
    public float maxLightRange = 20f;
    public float maxLightIntensity;
    public float maxFuel = 10f;
    public float maxEffectiveFuel = 5f;
    public float fuelConsumptionPerSecond = 1f;
    public AnimationCurve lightIntensityCurve = new(new Keyframe(0f, 1f), new Keyframe(0.75f, 1f), new Keyframe(1f, 0f));
    public AnimationCurve lightRangeCurve = new(new Keyframe(0f, 1f), new Keyframe(0.75f, 1f), new Keyframe(1f, 0f));
    public AnimationCurve totalSafetyCurve = new(new Keyframe(0f, 1f), new Keyframe(0.75f, 1f), new Keyframe(1f, 0f));
    public FireEffects fireEffects;
    [Header("Fluff")]
    public string interactTextAble = "Tend to the fire";
    public string interactTextUnable = "You have nothing left";
    public Sprite interactImageAble;
    public Sprite interactImageUnable;
    [Header("State")]
    [Fade] public List<Fuel> activeFuel;
    [Fade] public float currentFuel = 0f;
    [Fade] public float currentLightIntensity = 0f;
    [Fade] public float currentLightRange = 0f;
    [Fade] public float currentMultiplier = 0f;
    [Fade] public float currentTotalSafetyRange = 0f;

    private PlayerInventory playerInventory;

    void OnDrawGizmos() {
        Gizmos.color = Color.orange;
        Gizmos.DrawWireSphere(transform.position, currentLightRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, currentTotalSafetyRange);
    }

    void Start() {
        playerInventory = FindFirstObjectByType<PlayerInventory>();
    }

    public override void Update() {
        base.Update();

        DecreaseActiveFuel();
        SetCurrentFuel();
        SetFireSize();

        #if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.P)) AddEditorFuel(5);
        #endif
    }

    public override void Interact() {
        AddFuelFromPlayerInventory();
    }

    public override string GetInteractText() {
        return playerInventory.IsCarryingAnyFuel() ? interactTextAble : interactTextUnable;
    }

    public override Sprite GetInteractImage() {
        return playerInventory.IsCarryingAnyFuel() ? interactImageAble : interactImageUnable;
    }

    private void SetCurrentFuel() {
        float total = 0f;

        foreach (Fuel fuel in activeFuel) {
            total += fuel.currentFuel;
        }

        currentFuel = total;
    }

    private void SetFireSize() {
        currentMultiplier = 1f - (1f / Mathf.Min(maxFuel, maxEffectiveFuel) * Mathf.Min(currentFuel, maxEffectiveFuel));

        currentLightRange = maxLightRange * lightRangeCurve.Evaluate(currentMultiplier);
        currentLightIntensity = maxLightIntensity * lightIntensityCurve.Evaluate(currentMultiplier);
        currentTotalSafetyRange = maxLightRange * totalSafetyCurve.Evaluate(currentMultiplier);
    }

    private void DecreaseActiveFuel() {
        foreach (Fuel fuel in activeFuel) {
            fuel.DecreaseCurrentFuel(Time.deltaTime * fuelConsumptionPerSecond / activeFuel.Count);
        }

        activeFuel = activeFuel.Where(fuel => fuel.currentFuel > 0).ToList();
    }

    private void AddFuelFromPlayerInventory() {
        if (!playerInventory.IsCarryingAnyFuel()) return;

        Fuel fuel = playerInventory.carryingFuel[0];

        playerInventory.UseFuel(fuel, this);

        fireEffects.BurstEmbers(Mathf.Min((activeFuel.Count - 1) * Mathf.CeilToInt(fuel.maxFuel / 10), 5));
    }

    public void AddEditorFuel(float amount) {
        GameObject fuelObject = new("Editor created fuel");
        fuelObject.AddComponent<Fuel>();
        Fuel fuel = fuelObject.GetComponent<Fuel>();
        fuel.maxFuel = amount;

        activeFuel.Add(fuel);
    }
}

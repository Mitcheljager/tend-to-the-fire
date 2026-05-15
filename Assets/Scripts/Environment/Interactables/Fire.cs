using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fire : Interactable {
    [Header("Config")]
    public float maxLightRange = 20f;
    public float maxLightIntensity;
    public float maxFuel = 10f;
    public float maxEffectiveFuel = 5f;
    public float fuelConsumptionPerSecond = 1f;
    public AnimationCurve lightIntensityCurve = new(new Keyframe(0f, 1f), new Keyframe(0.75f, 1f), new Keyframe(1f, 0f));
    public AnimationCurve lightRangeCurve = new(new Keyframe(0f, 1f), new Keyframe(0.75f, 1f), new Keyframe(1f, 0f));
    [Header("Objects")]
    public Light fireLight;
    [Header("Effects")]
    public Renderer[] effectRenderers;
    public AnimationCurve effectFuzzinessCurve = new(new Keyframe(0f, 1f), new Keyframe(0.75f, 1f), new Keyframe(1f, 0f));
    public AnimationCurve effectContrastCurve = new(new Keyframe(0f, 1f), new Keyframe(0.75f, 1f), new Keyframe(1f, 0f));
    public AnimationCurve effectIntensityCurve = new(new Keyframe(0f, 1f), new Keyframe(0.75f, 1f), new Keyframe(1f, 0f));
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

    private PlayerInventory playerInventory;

    void OnDrawGizmos() {
        Gizmos.color = Color.orange;
        Gizmos.DrawWireSphere(transform.position, currentLightRange);
    }

    void Start() {
        playerInventory = FindFirstObjectByType<PlayerInventory>();
    }

    void Update() {
        DecreaseActiveFuel();
        SetCurrentFuel();
        SetFireSize();
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
        float multiplier = 1f - (1f / Mathf.Min(maxFuel, maxEffectiveFuel) * Mathf.Min(currentFuel, maxEffectiveFuel));

        currentLightRange = maxLightRange * lightRangeCurve.Evaluate(multiplier);
        currentLightIntensity = maxLightIntensity * lightIntensityCurve.Evaluate(multiplier);

        SetEffectValues(multiplier);
    }

    private void SetEffectValues(float multiplier) {
        foreach (Renderer effectRenderer in effectRenderers) {
            effectRenderer.material.SetFloat("_Fuzziness", effectFuzzinessCurve.Evaluate(multiplier));
            effectRenderer.material.SetFloat("_Contrast", effectContrastCurve.Evaluate(multiplier));
            effectRenderer.material.SetFloat("_Intensity", effectIntensityCurve.Evaluate(multiplier));
        }
    }

    private void DecreaseActiveFuel() {
        foreach (Fuel fuel in activeFuel) {
            fuel.DecreaseCurrentFuel(Time.deltaTime * fuelConsumptionPerSecond);
        }

        activeFuel = activeFuel.Where(fuel => fuel.currentFuel > 0).ToList();
    }

    private void AddFuelFromPlayerInventory() {
        if (!playerInventory.IsCarryingAnyFuel()) return;

        Fuel fuel = playerInventory.carryingFuel[0];

        playerInventory.UseFuel(fuel, this);
    }
}

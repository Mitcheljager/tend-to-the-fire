using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightFlicker : MonoBehaviour {
    [Header("Intensity")]
    public float minimumIntensityMultiplier = 0f;
    public float maximumIntensityMultiplier = 2f;
    [Header("Position")]
    public Vector3 minimumPositionDisplacement = new(-1, 0, -1);
    public Vector3 maximumPositionDisplacement = new(1, 0, 1);
    [Header("Smoothness")]
    public float lerpChance = 0.1f;
    public float lerpSpeed = 8f;

    private Light lightComponent;
    private float baseIntensity = 0f;
    private float baseRange = 0f;
    private Vector3 basePosition = new();
    private Vector3 targetPosition = new();
    private float currentIntensityMultiplier = 0;
    private float targetIntensityMultiplier = 1f;

    void Start() {
        lightComponent = GetComponent<Light>();

        baseIntensity = lightComponent.intensity;
        baseRange = lightComponent.range;
        basePosition = transform.position;
    }

    void Update() {
        if (!lightComponent.enabled) return;

        CycleIntensity();
    }

    void CycleIntensity() {
        if (Random.value < lerpChance) {
            targetIntensityMultiplier = Random.Range(minimumIntensityMultiplier, maximumIntensityMultiplier);
            targetPosition = basePosition + new Vector3(
                Random.Range(minimumPositionDisplacement.x, maximumPositionDisplacement.x),
                Random.Range(minimumPositionDisplacement.y, maximumPositionDisplacement.y),
                Random.Range(minimumPositionDisplacement.z, maximumPositionDisplacement.z)
            );
        }

        currentIntensityMultiplier = Mathf.Lerp(currentIntensityMultiplier, targetIntensityMultiplier, Time.deltaTime * lerpSpeed);
        lightComponent.intensity = baseIntensity * currentIntensityMultiplier;
        lightComponent.range = baseRange * currentIntensityMultiplier;

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * lerpSpeed);;
    }
}

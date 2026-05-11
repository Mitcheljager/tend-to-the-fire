using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour {
    public float minimumMultiplier = 0f;
    public float maximumMultiplier = 2f;
    public float lerpChance = 0.1f;
    public float lerpSpeed = 8f;

    private Light light;
    private float currentMultiplier = 0;
    private float baseIntensity = 0f;
    private float baseRange = 0f;
    private float targetMultiplier = 1f;

    void Start() {
        light = GetComponent<Light>();

        baseIntensity = light.intensity;
        baseRange = light.range;
    }

    void Update() {
        if (!light.enabled) return;

        CycleIntensity();
    }

    void CycleIntensity() {
        if (Random.value < lerpChance) targetMultiplier = Random.Range(minimumMultiplier, maximumMultiplier);

        currentMultiplier = Mathf.Lerp(currentMultiplier, targetMultiplier, Time.deltaTime * lerpSpeed);
        light.intensity = baseIntensity * currentMultiplier;
        light.range = baseRange * currentMultiplier;
    }
}

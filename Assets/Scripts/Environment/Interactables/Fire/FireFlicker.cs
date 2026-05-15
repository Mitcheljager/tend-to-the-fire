using UnityEngine;

[RequireComponent(typeof(Light))]
public class FireFlicker : MonoBehaviour {
    [Header("Components")]
    public Fire fire;
    [Header("Intensity")]
    public float minimumIntensityMultiplier = 0f;
    public float maximumIntensityMultiplier = 2f;
    public float minimumRangeMultiplier = 0f;
    public float maximumRangeMultiplier = 2f;
    [Header("Position")]
    public Vector3 minimumPositionDisplacement = new(-1, 0, -1);
    public Vector3 maximumPositionDisplacement = new(1, 0, 1);
    [Header("Smoothness")]
    public float lerpChance = 0.1f;
    public float lerpSpeed = 8f;

    private Light lightComponent;
    private Vector3 basePosition = new();
    private Vector3 targetPosition = new();
    private float currentIntensityMultiplier = 0;
    private float targetIntensityMultiplier = 1f;
    private float currentRangeMultiplier = 0;
    private float targetRangeMultiplier = 1f;

    void Start() {
        lightComponent = GetComponent<Light>();

        basePosition = transform.position;
    }

    void Update() {
        if (lightComponent.enabled) CycleIntensity();
    }

    void CycleIntensity() {
        if (targetPosition == Vector3.zero || Random.value < lerpChance) {
            targetIntensityMultiplier = Random.Range(minimumIntensityMultiplier, maximumIntensityMultiplier);
            targetRangeMultiplier = Random.Range(minimumRangeMultiplier, maximumRangeMultiplier);
            targetPosition = basePosition + new Vector3(
                Random.Range(minimumPositionDisplacement.x, maximumPositionDisplacement.x),
                Random.Range(minimumPositionDisplacement.y, maximumPositionDisplacement.y),
                Random.Range(minimumPositionDisplacement.z, maximumPositionDisplacement.z)
            );
        }

        currentIntensityMultiplier = Mathf.Lerp(currentIntensityMultiplier, targetIntensityMultiplier, Time.deltaTime * lerpSpeed);
        lightComponent.intensity = fire.currentLightIntensity * currentIntensityMultiplier;

        currentRangeMultiplier = Mathf.Lerp(currentRangeMultiplier, targetRangeMultiplier, Time.deltaTime * lerpSpeed);
        lightComponent.range = fire.currentLightRange * currentRangeMultiplier;

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * lerpSpeed);;
    }
}

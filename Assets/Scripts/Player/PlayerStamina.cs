using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStamina : MonoBehaviour {
    [Header("Config")]
    public float lossPerSecond = 0.1f;
    public float gainPerSecond = 0.1f;
    public float secondsRecovery = 2f;
    [Range(0, 1f)] public float maxLimiterByInventoryWeight = 0.5f;
    public AnimationCurve limiterLossCurve = new(new Keyframe(0f, 1f), new Keyframe(0.75f, 1f), new Keyframe(1f, 0f));
    [Header("Animation")]
    public AnimationCurve opacityCurve = new(new Keyframe(0f, 1f), new Keyframe(0.75f, 1f), new Keyframe(1f, 0f));
    public Animation recoveryAnimation;
    [Header("Components")]
    public CanvasGroup barCanvasGroup;
    public Slider currentStaminaSlider;
    public Slider currentWeightLimiterSlider;
    public GameObject[] hideObjectsOnEmpty;
    public GameObject[] hideObjectsOnFull;
    [Header("State")]
    [Fade] public float currentStamina = 1f;
    [Fade] public float currentTotalLimiter = 0f;
    [Fade] public float currentWeightLimiter = 0f;
    [Fade] public bool isRecovering = false;

    private PlayerMovement playerMovement;
    private PlayerInventory playerInventory;
    private readonly float maxStamina = 1f;
    private bool wasRunning = false;

    void Start() {
        playerMovement = FindFirstObjectByType<PlayerMovement>();
        playerInventory = FindFirstObjectByType<PlayerInventory>();
    }

    void Update() {
        SetLimiter();

        if (isRecovering) return;

        SetStaminaValue();
        SetStaminaUI();
    }

    private void SetStaminaValue() {
        if (playerMovement.isRunning) {
            wasRunning = true;
            currentStamina = Mathf.Max(currentStamina - Time.deltaTime * lossPerSecond, 0f);

            if (currentStamina <= 0f) StartCoroutine(SetRecovering());
        } else {
            currentStamina = Mathf.Min(currentStamina + Time.deltaTime * gainPerSecond, maxStamina);

            if (wasRunning) StartCoroutine(SetRecovering());
        }

        currentStamina = Mathf.Min(currentStamina, maxStamina - currentTotalLimiter);
    }

    private void SetStaminaUI() {
        barCanvasGroup.alpha = opacityCurve.Evaluate(currentStamina);

        currentStaminaSlider.value = currentStamina;
        currentWeightLimiterSlider.value = currentWeightLimiter;

        foreach(GameObject gameObject in hideObjectsOnEmpty) {
            gameObject.SetActive(currentStamina > 0f);
        }

        foreach(GameObject gameObject in hideObjectsOnFull) {
            gameObject.SetActive(currentStamina < 1f);
        }
    }

    private void SetLimiter() {
        float weightMultiplier = 1f / playerInventory.maxWeight * playerInventory.currentWeight;

        currentWeightLimiter = maxLimiterByInventoryWeight * weightMultiplier;
        currentTotalLimiter = currentWeightLimiter;
    }

    private IEnumerator SetRecovering() {
        isRecovering = true;

        recoveryAnimation.Play();

        yield return new WaitForSeconds(secondsRecovery);

        isRecovering = false;
        wasRunning = false;
    }
}

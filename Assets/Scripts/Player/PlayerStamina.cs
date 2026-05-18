using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStamina : MonoBehaviour {
    [Header("Config")]
    public float lossPerSecond = 0.1f;
    public float gainPerSecond = 0.1f;
    public float secondsRecovery = 2f;
    public AnimationCurve limiterLossCurve = new(new Keyframe(0f, 1f), new Keyframe(0.75f, 1f), new Keyframe(1f, 0f));
    [Header("Animation")]
    public AnimationCurve opacityCurve = new(new Keyframe(0f, 1f), new Keyframe(0.75f, 1f), new Keyframe(1f, 0f));
    public Animation recoveryAnimation;
    [Header("Components")]
    public CanvasGroup barCanvasGroup;
    public Slider barSlider;
    public GameObject[] hideObjectsOnEmpty;
    [Header("State")]
    [Fade] public float currentStamina = 1f;
    [Fade] public float currentStaminaLimiter = 0f;
    [Fade] public bool isRecovering = false;

    private PlayerMovement playerMovement;
    private readonly float maxStamina = 1f;
    private bool wasRunning = false;

    void Start() {
        playerMovement = FindFirstObjectByType<PlayerMovement>();
    }

    void Update() {
        if (isRecovering) return;

        if (playerMovement.isRunning) {
            wasRunning = true;
            currentStamina = Mathf.Max(currentStamina - Time.deltaTime * lossPerSecond, 0f);

            if (currentStamina <= 0f) StartCoroutine(SetRecovering());
        } else {
            currentStamina = Mathf.Min(currentStamina + Time.deltaTime * gainPerSecond, maxStamina);

            if (wasRunning) StartCoroutine(SetRecovering());
        }

        barSlider.value = currentStamina;
        barCanvasGroup.alpha = opacityCurve.Evaluate(currentStamina);

        foreach(GameObject gameObject in hideObjectsOnEmpty) {
            gameObject.SetActive(currentStamina > 0f);
        }
    }

    private IEnumerator SetRecovering() {
        isRecovering = true;

        recoveryAnimation.Play();

        yield return new WaitForSeconds(secondsRecovery);

        isRecovering = false;
        wasRunning = false;
    }
}

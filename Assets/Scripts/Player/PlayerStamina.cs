using System.Collections;
using UnityEngine;

public class PlayerStamina : MonoBehaviour {
    [Header("Config")]
    public float lossPerSecond = 0.1f;
    public float recoveryPerSecond = 0.1f;
    public float secondsExhausted = 2f;
    public AnimationCurve limiterLossCurve = new(new Keyframe(0f, 1f), new Keyframe(0.75f, 1f), new Keyframe(1f, 0f));
    [Header("State")]
    [Fade] public float currentStamina = 1f;
    [Fade] public float currentStaminaLimiter = 0f;
    [Fade] public bool isExhausted = false;

    private PlayerMovement playerMovement;
    private readonly float maxStamina = 1f;

    void Start() {
        playerMovement = FindFirstObjectByType<PlayerMovement>();
    }

    void Update() {
        if (isExhausted) return;

        if (playerMovement.isRunning) {
            currentStamina = Mathf.Max(currentStamina - Time.deltaTime * lossPerSecond, 0f);

            if (currentStamina <= 0f) StartCoroutine(SetExhausted());
        } else {
            currentStamina = Mathf.Min(currentStamina + Time.deltaTime * recoveryPerSecond, maxStamina);
        }
    }

    private IEnumerator SetExhausted() {
        isExhausted = true;
        currentStamina = 0f;

        yield return new WaitForSeconds(secondsExhausted);

        isExhausted = false;
    }
}

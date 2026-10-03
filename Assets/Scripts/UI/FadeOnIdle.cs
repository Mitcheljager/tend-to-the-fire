using UnityEngine;
using UnityEngine.InputSystem;

public class FadeOnIdle : MonoBehaviour {
    public float idleSeconds = 3f;
    public float fadeTimeSeconds = 2f;
    public UIAnimationHelper uiAnimationHelper;

    private float currentIdleTime = 0f;
    private bool isCurrentlyIdle = false;

    void Update() {
        if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame || Mouse.current != null && Mouse.current.delta.magnitude > 0.01f || Gamepad.current != null && Gamepad.current.wasUpdatedThisFrame) {
            if (!isCurrentlyIdle) return;

            currentIdleTime = 0f;
            isCurrentlyIdle = false;
            uiAnimationHelper.FadeIn(0f);

            return;
        }

        currentIdleTime += Time.deltaTime;

        if (currentIdleTime < idleSeconds) return;
        if (isCurrentlyIdle) return;

        isCurrentlyIdle = true;
        uiAnimationHelper.FadeOut(fadeTimeSeconds);
    }
}

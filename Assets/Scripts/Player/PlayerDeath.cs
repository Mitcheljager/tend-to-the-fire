using System.Collections;
using UnityEngine;

public class PlayerDeath : MonoBehaviour {
    [Header("Objects")]
    public PlayerCamera playerCamera;
    public PlayerState playerState;
    public PlayerFocus playerFocus;
    [Header("Config")]
    public float targetFieldOfView = 180f;
    public float fieldOfViewDurationSeconds = 1f;
    public float setFocusAfterSeconds = 0.5f;

    private void OnEnable() {
        PlayerEvent.OnPlayerDiedEvent.AddListener(PlayDeathSequence);
    }

    private void OnDisable() {
        PlayerEvent.OnPlayerDiedEvent.RemoveListener(PlayDeathSequence);
    }

    private void PlayDeathSequence() {
        StartCoroutine(DeathSequenceCoroutine());
    }

    private IEnumerator DeathSequenceCoroutine() {
        float totalFieldOfViewDifference = targetFieldOfView - playerCamera.thisCamera.fieldOfView;
        float changePerSecond = 1f / fieldOfViewDurationSeconds * targetFieldOfView;;
        float elapsedTime = 0f;
        bool hasFocusBeenSet = false;

        playerState.SetInStasis(true);

        while (elapsedTime < fieldOfViewDurationSeconds) {
            playerCamera.thisCamera.fieldOfView += changePerSecond * Time.deltaTime;

            elapsedTime += Time.deltaTime;

            if (!hasFocusBeenSet && elapsedTime >= setFocusAfterSeconds) {
                playerFocus.SetIsForced(true);
                hasFocusBeenSet = true;
            }

            yield return new WaitForEndOfFrame();
        }
    }
}

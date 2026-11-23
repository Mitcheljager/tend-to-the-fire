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
        float totalFieldOfViewDifference = playerCamera.thisCamera.fieldOfView;
        float elapsedTime = 0f;
        bool hasFocusBeenSet = false;

        playerState.SetInStasis(true);

        while (playerCamera.thisCamera.fieldOfView <= targetFieldOfView) {
            playerCamera.thisCamera.fieldOfView += 1f / fieldOfViewDurationSeconds * Time.deltaTime * targetFieldOfView;

            yield return new WaitForEndOfFrame();

            elapsedTime += Time.deltaTime;

            if (!hasFocusBeenSet && elapsedTime >= setFocusAfterSeconds) {
                playerFocus.SetIsForced(true);
                hasFocusBeenSet = true;
            }
        }
    }
}

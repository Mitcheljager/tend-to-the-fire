using UnityEngine;

public class PlayerFootsteps : MonoBehaviour {
    [Header("Objects")]
    public PlayerMovement playerMovement;
    public AudioHelper audioHelperWalking;
    public AudioHelper audioHelperRunning;
    [Header("Config")]
    public float footstepCooldownWalking = 0.75f;
    public float footstepCooldownRunning = 0.5f;
    [Header("State")]
    public float lastFootstepSecondsAgo = 0f;

    void Update() {
        if (!playerMovement.isGrounded) return;

        if (playerMovement.move.magnitude == 0) {
            lastFootstepSecondsAgo = 0f;
            return;
        }

        lastFootstepSecondsAgo += Time.deltaTime;

        float footstepCooldown = playerMovement.isRunning ? footstepCooldownRunning : footstepCooldownWalking;

        if (lastFootstepSecondsAgo < footstepCooldown) return;

        if (playerMovement.isRunning) audioHelperRunning.PlayRandomClip();
        else audioHelperWalking.PlayRandomClip();

        lastFootstepSecondsAgo = 0f;
    }
}

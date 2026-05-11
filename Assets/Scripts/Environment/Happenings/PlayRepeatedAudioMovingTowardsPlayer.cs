using UnityEngine;

[RequireComponent(typeof(AudioHelper))]
public class PlayRepeatedAudioMovingTowardsPlayer : MonoBehaviour {
    [Header("Config")]
    public float distancePerSecond = 1f;
    public float timeBetweenAudio = 0.5f;
    public float minimumDistance = 10f;
    public bool disableOnReached = false;

    private PlayerState playerState;
    private AudioHelper audioHelper;
    private float audioTimer = 0f;

    void Start() {
        playerState = FindFirstObjectByType<PlayerState>();
        audioHelper = GetComponent<AudioHelper>();
    }

    void Update() {
        float currentDistance = Vector3.Distance(transform.position, playerState.transform.position);

        if (currentDistance < minimumDistance) {
            if (disableOnReached) gameObject.SetActive(false);
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, playerState.transform.position, distancePerSecond * Time.deltaTime);

        audioTimer -= Time.deltaTime;
        if (audioTimer > 0f) return;

        audioHelper.PlayRandomClip();
        audioTimer = timeBetweenAudio;
    }
}

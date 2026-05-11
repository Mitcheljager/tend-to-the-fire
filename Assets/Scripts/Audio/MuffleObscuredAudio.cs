using UnityEngine;

public class MuffleObscuredAudio : MonoBehaviour {
    public LayerMask layerMask;
    public AudioReverbZone audioReverbZone;
    [Header("State")]
    public bool obscured = false;

    private PlayerState playerState;
    private AudioSource audioSource;
    private AudioLowPassFilter audioLowPassFilter;

    private float initialVolume;

    void Start() {
        playerState = FindAnyObjectByType<PlayerState>();
        audioLowPassFilter = GetComponent<AudioLowPassFilter>();
        audioSource = GetComponent<AudioSource>();

        initialVolume = audioSource.volume;
    }

    void Update() {
        Vector3 direction = playerState.transform.position - transform.position;
        float distance = direction.magnitude;

        if (Physics.Raycast(transform.position, direction.normalized, distance, layerMask)) {
            obscured = true;
        } else {
            obscured = false;
        }

        audioLowPassFilter.enabled = obscured;
        audioReverbZone.enabled = obscured;
        audioSource.volume = obscured ? initialVolume / 2 : initialVolume;
    }
}

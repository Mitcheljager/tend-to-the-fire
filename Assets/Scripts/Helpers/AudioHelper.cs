using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioHelper : MonoBehaviour {
    [Header("Config")]
    public float minPitch = 1f;
    public float maxPitch = 1f;
    public float delay = 0f;
    public bool playAtPoint = false;
    public bool avoidRepeats = false;
    [Header("Clips")]
    public AudioClip[] audioClips;
    [Header("State")]
    [Fade] public AudioSource audioSource;
    [Fade][Tooltip("Modify the given pitch by a multiplier, only controlled through script")] public float pitchMultiplier = 1f;

    private int lastIndex = -1;

    private class PropertyRange {
        public float min;
        public float max;

        public PropertyRange(float min, float max) {
            this.min = min;
            this.max = max;
        }
    }

    void OnDrawGizmosSelected() {
        Debug.DrawLine(transform.position, Camera.main.transform.position);
    }

    void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayRandomClip() {
        if (delay > 0f) {
            StartCoroutine(PlayDelayed());
        } else {
            Play();
        }
    }

    private void Play() {
        AudioClip randomClip = GetRandomPitchedClip();

        if (playAtPoint) {
            AudioSource.PlayClipAtPoint(randomClip, transform.position, audioSource.volume);
        } else {
            audioSource.PlayOneShot(randomClip);
        }
    }

    private AudioClip GetRandomPitchedClip() {
        if (audioClips.Length == 0) return null;

        int randomIndex = GetRandomClipIndex();
        while (avoidRepeats && randomIndex == lastIndex && audioClips.Length > 1) {
            randomIndex = GetRandomClipIndex();
        }

        audioSource.pitch = Random.Range(minPitch, maxPitch) * pitchMultiplier;
        lastIndex = randomIndex;

        return audioClips[randomIndex];
    }

    private int GetRandomClipIndex() {
        return Random.Range(0, audioClips.Length);
    }

    private IEnumerator PlayDelayed() {
        yield return new WaitForSeconds(delay);

        Play();
    }
}

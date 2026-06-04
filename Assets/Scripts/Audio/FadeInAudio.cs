using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FadeInAudio : MonoBehaviour {
    public float fadeTimeSeconds = 1f;

    private AudioSource audioSource;
    private float volumeTarget = 0f;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        volumeTarget = audioSource.volume;
        audioSource.volume = 0f;

        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn() {
        float currentTime = 0f;

        while (audioSource.volume < volumeTarget) {
            audioSource.volume = Mathf.Lerp(0, volumeTarget, currentTime / fadeTimeSeconds);
            currentTime += Time.deltaTime;

            yield return null;
        }
    }
}

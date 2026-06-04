using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FluctuateAudioVolume : MonoBehaviour {
    public float initialDelaySeconds = 5f;
    public Vector2 volumeRange = new(0.75f, 1f);
    public Vector2 fadeTimeRangeSeconds = new(5f, 10f);
    public Vector2 fadeWaitRangeSeconds = new(5f, 10f);

    private AudioSource audioSource;

    void Start() {
        audioSource = GetComponent<AudioSource>();

        StartCoroutine(Fluctuate());
    }

    private IEnumerator Fluctuate() {
        yield return new WaitForSeconds(initialDelaySeconds);

        while (true) {
            float currentTime = 0f;
            float volumeStart = audioSource.volume;
            float volumeTarget = Random.Range(volumeRange.x, volumeRange.y);
            float fadeTimeSeconds = Random.Range(fadeTimeRangeSeconds.x, fadeTimeRangeSeconds.y);

            while (currentTime < fadeTimeSeconds) {
                audioSource.volume = Mathf.Lerp(volumeStart, volumeTarget, currentTime / fadeTimeSeconds);
                currentTime += Time.deltaTime;

                yield return null;
            }

            yield return new WaitForSeconds(Random.Range(fadeWaitRangeSeconds.x, fadeWaitRangeSeconds.y));
        }
    }
}

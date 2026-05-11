using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioHelper : MonoBehaviour {
    [Header("AudioHelper Config")]
    public float minPitch = 1f;
    public float maxPitch = 1f;
    public bool playAtPoint = false;

    [Header("AudioHelper Optional")]
    public AudioClip[] audioClips;

    public bool doReverb = true;

    public AudioSource audioSource;

    private GameObject player;
    private AudioConnectorManager audioConnectorManager;
    private AudioConnector bestAudioConnector;

    private AudioClip randomClip;

    private class PropertyRange {
        public float min;
        public float max;

        public PropertyRange(float min, float max) {
            this.min = min;
            this.max = max;
        }
    }

    private readonly Dictionary<string, PropertyRange> reverbValues = new() {
        { "decayHFRatio", new PropertyRange(1f, 0.2f) },
        { "decayTime", new PropertyRange(1f, 10f) },
        { "density", new PropertyRange(0f, 100f) },
        { "diffusion", new PropertyRange(0f, 100f) },
        { "reflectionsLevel", new PropertyRange(-2600f, 0f) },
        { "reverbDelay", new PropertyRange(0.011f, 0.03f) },
        { "reverbLevel", new PropertyRange(200f, -2f) },
        { "room", new PropertyRange(-10000f, 0f) },
        { "roomHF", new PropertyRange(-10000f, 0f) },
    };

    void Start() {
        audioSource = GetComponent<AudioSource>();
        player = GameObject.Find("Player");
    }

    void Update() {
        Debug.DrawLine(audioSource.transform.position, player.transform.position);
    }

    void OnDrawGizmos() {
        if (bestAudioConnector != null) {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(bestAudioConnector.transform.position, 0.2f);
        }
    }

    public void PlayRandomClip() {
        SetRandomPitchedClip();

        if (playAtPoint) {
            AudioSource.PlayClipAtPoint(randomClip, transform.position, audioSource.volume);
        } else {
            audioSource.PlayOneShot(randomClip);
        }
    }

    public void PlayAudioViaConnector() {
        if (audioConnectorManager == null) audioConnectorManager = GameObject.Find("Audio Connector Manager").GetComponent<AudioConnectorManager>();

        float playerDistance = Vector3.Distance(player.transform.position, audioSource.transform.position);
        if (playerDistance > audioSource.maxDistance) return;

        (AudioConnector _bestAudioConnector, float distance) = audioConnectorManager.FindBestAudioConnector(audioSource.transform.position);
        bestAudioConnector = _bestAudioConnector;

        float clampedDistance = Mathf.Clamp(distance, audioSource.minDistance, audioSource.maxDistance);
        float normalizedDistance = (audioSource.maxDistance - clampedDistance) / (audioSource.maxDistance - audioSource.minDistance);
        float volume = audioSource.volume * normalizedDistance;
        Vector3 position = bestAudioConnector == null ? audioSource.transform.position : bestAudioConnector.transform.position;

        SetRandomPitchedClip();
        PlayClonedClip(randomClip, position, volume, distance, bestAudioConnector != null);
    }

    private void SetRandomPitchedClip() {
        audioSource.pitch = Random.Range(minPitch, maxPitch);
        randomClip = audioClips[Random.Range(0, audioClips.Length)];
    }

    private void PlayClonedClip(AudioClip clip, Vector3 position, float volume, float distance, bool useLowPass) {
        GameObject temporaryObject = new("Play Cloned Clip");
        AudioSource clone = temporaryObject.AddComponent<AudioSource>();

        temporaryObject.transform.position = position;

        clone.volume = volume;
        clone.outputAudioMixerGroup = audioSource.outputAudioMixerGroup;
        clone.spatialBlend = audioSource.spatialBlend;
        clone.minDistance = audioSource.minDistance;
        clone.maxDistance = audioSource.maxDistance;
        clone.rolloffMode = audioSource.rolloffMode;
        clone.playOnAwake = false;

        float filterMaxDistance = 30f;
        float normalizedDistance = distance / filterMaxDistance;

        if (useLowPass && audioSource.gameObject.TryGetComponent<AudioLowPassFilter>(out var lowPassFilter)) {
            AudioLowPassFilter cloneLowPassFilter = temporaryObject.AddComponent<AudioLowPassFilter>();
            float lowPassFrequencyMin = 1500f;
            float lowPassFrequencyMax = 8000f;

            float invertedNormalizedDistance = 1 - normalizedDistance;
            float frequency = Mathf.Lerp(lowPassFrequencyMin, lowPassFrequencyMax, invertedNormalizedDistance);

            cloneLowPassFilter.cutoffFrequency = Mathf.Clamp(frequency, lowPassFrequencyMin, lowPassFrequencyMax);
            cloneLowPassFilter.lowpassResonanceQ = lowPassFilter.lowpassResonanceQ;
        }

        if (doReverb && audioSource.gameObject.GetComponent<AudioReverbFilter>()) {
            AudioReverbFilter cloneReverbFilter = temporaryObject.AddComponent<AudioReverbFilter>();

            foreach (var entry in reverbValues) {
                float normalizedValue = entry.Value.min + ((entry.Value.max - entry.Value.min) * normalizedDistance);
                var propertyInfo = typeof(AudioReverbFilter).GetProperty(entry.Key);

                if (propertyInfo != null && propertyInfo.CanWrite) {
                    propertyInfo.SetValue(cloneReverbFilter, normalizedValue);
                }
            }
        }

        clone.PlayOneShot(clip);

        Destroy(temporaryObject, clip.length);
    }
}

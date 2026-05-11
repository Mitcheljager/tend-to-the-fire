using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioReverbFilter))]
public class ReverbDistantAudio : MonoBehaviour {
    private PlayerState playerState;
    private AudioSource audioSource;
    private AudioReverbFilter reverbFilter;

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
        playerState = FindAnyObjectByType<PlayerState>();
        audioSource = GetComponent<AudioSource>();
        reverbFilter = GetComponent<AudioReverbFilter>();
    }

    void Update() {
        // Only set while not playing to avoid the sound shifting as it plays
        if (!audioSource.isPlaying || audioSource.loop) SetReverb();
    }

    private void SetReverb() {
        float maxDistance = 30f;
        float distance = Vector3.Distance(transform.position, playerState.transform.position);

        if (distance > audioSource.maxDistance) return;

        float normalizedDistance = Mathf.Clamp01(distance / maxDistance);

        foreach (var entry in reverbValues) {
            float normalizedValue = entry.Value.min + ((entry.Value.max - entry.Value.min) * normalizedDistance);
            typeof(AudioReverbFilter).GetProperty(entry.Key).SetValue(reverbFilter, normalizedValue);
        }
    }
}

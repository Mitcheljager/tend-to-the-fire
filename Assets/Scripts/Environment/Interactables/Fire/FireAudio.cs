using UnityEngine;

public class FireAudio : MonoBehaviour {
    public Fire fire;
    [Header("Audio")]
    public AudioHelper[] audioHelpersBase;
    public AnimationCurve volumeBaseCurve = new(new Keyframe(0f, 1f), new Keyframe(0.75f, 1f), new Keyframe(1f, 0f));

    void Update() {
        SetAudioValues();
    }

    private void SetAudioValues() {
        foreach (AudioHelper audioHelper in audioHelpersBase) {
            audioHelper.audioSource.volume = volumeBaseCurve.Evaluate(fire.currentMultiplier);
        }
    }
}

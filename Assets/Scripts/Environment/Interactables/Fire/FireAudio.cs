using UnityEngine;

[System.Serializable]
public class FireAudioItem {
    public AudioHelper audioHelper;
    public AnimationCurve volumeCurve = new(new Keyframe(0f, 1f), new Keyframe(0.75f, 1f), new Keyframe(1f, 0f));
}

public class FireAudio : MonoBehaviour {
    public Fire fire;
    public FireAudioItem[] audioItems;
    public AudioHelper audioHelperWoosh;

    void Update() {
        SetAudioValues();
    }

    private void SetAudioValues() {
        foreach (FireAudioItem item in audioItems) {
            item.audioHelper.audioSource.volume = item.volumeCurve.Evaluate(fire.currentMultiplier);
        }
    }
}

using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioHelper))]
public class RepeatedAudio : MonoBehaviour {
    public float delayInSeconds = 1f;
    public bool viaConnector = true;

    private AudioHelper audioHelper;

    void Start() {
        audioHelper = GetComponent<AudioHelper>();

        StartCoroutine(RepeatAudio());
    }

    private IEnumerator RepeatAudio() {
        while (true) {
            yield return new WaitForSeconds(delayInSeconds);

            if (viaConnector) audioHelper.PlayAudioViaConnector();
            else audioHelper.PlayRandomClip();
        }
    }
}

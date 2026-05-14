using UnityEngine;
using UnityEngine.Audio;

public class PlayerFocus : MonoBehaviour {
    [Header("References")]
    public RectTransform topEyeLid;
    public RectTransform bottomEyeLid;
    [Header("Animation")]
    public float animationSpeed = 5f;
    [Header("Audio")]
    public AudioMixer audioMixer;
    public int focusAudioBoost = 0;
    public int focusAudioDropoff = -60;
    [Header("State")]
    [Fade] public bool isClosed = false;
    [Fade] public bool isFullyClosed = false;

    void Update() {
        isClosed = Input.GetButton("Close Eyes");

        float screenHeight = Screen.height;

        float topTargetBottom = isClosed ? screenHeight * 0.5f : screenHeight;
        Vector2 topOffsetMin = topEyeLid.offsetMin;
        topOffsetMin.y = Mathf.Lerp(topOffsetMin.y, topTargetBottom - screenHeight, Time.deltaTime * animationSpeed);
        topEyeLid.offsetMin = topOffsetMin;

        float bottomTargetTop = isClosed ? screenHeight * 0.5f : 0;
        Vector2 bottomOffsetMax = bottomEyeLid.offsetMax;
        bottomOffsetMax.y = Mathf.Lerp(bottomOffsetMax.y, bottomTargetTop, Time.deltaTime * animationSpeed);
        bottomEyeLid.offsetMax = bottomOffsetMax;

        float targetTopOffsetY = topTargetBottom - screenHeight;
        bool topClosed = Mathf.Abs(topOffsetMin.y - targetTopOffsetY) <= 0.1f;
        bool bottomClosed = Mathf.Abs(bottomOffsetMax.y - bottomTargetTop) <= 0.1f;
        isFullyClosed = isClosed && topClosed && bottomClosed;

        audioMixer.SetFloat("FocusVolume", isClosed ? 0 : focusAudioDropoff);
        audioMixer.SetFloat("BoostVolume", isClosed ? focusAudioBoost : 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moonrise : MonoBehaviour {
    public AnimationCurve moonlightIntensityCurve = new(new Keyframe(0f, 1f), new Keyframe(0.75f, 1f), new Keyframe(1f, 0f));
    public Transform moonRotationTransform;
    public Light moonLight;
    public Quaternion rotateFrom;
    public Quaternion rotateMiddle;
    public Quaternion rotateTo;
    public float durationSeconds = 10f;

    private float initialMoonLightIntensity;

    void Start() {
        initialMoonLightIntensity = moonLight.intensity;

        StartCoroutine(StartMoonrise());
    }

    public void RestartMoonrise() {
        StopAllCoroutines();
        StartCoroutine(StartMoonrise());
    }

    private IEnumerator StartMoonrise() {
        float currentTime = 0f;

        while (currentTime < durationSeconds) {
            float lerp = 1f / durationSeconds * currentTime;

            if (lerp < 0.5f) moonRotationTransform.rotation = Quaternion.Lerp(rotateFrom, rotateMiddle, lerp * 2f);
            else moonRotationTransform.rotation = Quaternion.Lerp(rotateMiddle, rotateTo, (lerp - 0.5f) * 2f);

            moonLight.intensity = initialMoonLightIntensity * moonlightIntensityCurve.Evaluate(lerp);

            currentTime += Time.deltaTime;

            yield return null;
        }
    }
}

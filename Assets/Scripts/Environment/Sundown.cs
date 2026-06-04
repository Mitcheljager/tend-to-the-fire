using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sundown : MonoBehaviour {
    public Renderer[] renderers;
    public Light sunLight;
    public Light moonLight;
    public Quaternion rotateTowards;
    public float durationSeconds = 5f;
    public float moonLightShadowDurationSeconds = 2f;

    [Fade] public List<Material> rendererMaterials;
    [Fade] public List<float> materialAlphaInitialValues;

    private float initialSunLightIntensity;
    // private float initialMoonLightShadowStrength;
    private Quaternion initialRotation;

    void Start() {
        foreach(Renderer renderer in renderers) {
            rendererMaterials.Add(renderer.materials[0]);
            materialAlphaInitialValues.Add(renderer.materials[0].color.a);
        }

        initialSunLightIntensity = sunLight.intensity;
        // initialMoonLightShadowStrength = moonLight.shadowStrength;
        initialRotation = transform.rotation;

        StartCoroutine(StartSundown());
    }

    private IEnumerator StartSundown() {
        float currentTime = 0f;

        while (currentTime < durationSeconds) {
            int index = 0;
            float lerp = currentTime / durationSeconds;

            sunLight.intensity = Mathf.Lerp(initialSunLightIntensity, 0, lerp);
            transform.rotation = Quaternion.Lerp(initialRotation, rotateTowards, lerp);

            foreach(Material material in rendererMaterials) {
                float alpha = Mathf.Lerp(materialAlphaInitialValues[index], 0, lerp);
                material.color = new Color(material.color.r, material.color.g, material.color.b, alpha);

                index++;
            }

            currentTime += Time.deltaTime;

            yield return null;
        }

        gameObject.SetActive(false);

        // currentTime = 0f;

        // while (currentTime < moonLightShadowDurationSeconds) {
        //     moonLight.shadowStrength = Mathf.Lerp(0, initialMoonLightShadowStrength, currentTime / moonLightShadowDurationSeconds);

        //     currentTime += Time.deltaTime;

        //     yield return null;
        // }
    }
}

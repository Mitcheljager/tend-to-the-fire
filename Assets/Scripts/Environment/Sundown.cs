using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sundown : MonoBehaviour {
    public Renderer[] renderers;
    public Light sunLight;
    public Quaternion rotateTowards;
    public float durationSeconds = 5f;

    [Fade] public List<Material> rendererMaterials;
    [Fade] public List<float> materialAlphaInitialValues;

    private float initialSunLightIntensity;
    private Quaternion initialRotation;

    void Start() {
        foreach(Renderer renderer in renderers) {
            rendererMaterials.Add(renderer.materials[0]);
            materialAlphaInitialValues.Add(renderer.materials[0].color.a);
        }

        initialSunLightIntensity = sunLight.intensity;
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
    }
}

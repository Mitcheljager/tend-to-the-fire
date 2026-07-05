using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyClose : MonoBehaviour {
    public Renderer meshRenderer;
    public Transform meshTransform;
    public float maxDistance = 2f;
    public float maxScaleMultiplier = 2f;

    private Vector3 initialScale;

    void Start() {
        initialScale = meshTransform.localScale;
    }

    void OnTriggerEnter(Collider collider) {
        if (!collider.CompareTag("Player")) return;

        meshRenderer.material.SetInt("_ZTest", (int)CompareFunction.Always);
    }

    void OnTriggerStay(Collider collider) {
        if (!collider.CompareTag("Player")) return;

        float distance = Vector3.Distance(transform.position, collider.transform.position);

        if (distance > maxDistance) return;

        float normalizedDistance = 1f - (distance / maxDistance);
        meshTransform.localScale = initialScale * Mathf.Lerp(1f, maxScaleMultiplier, normalizedDistance);;
    }

    void OnTriggerExit(Collider collider) {
        if (!collider.CompareTag("Player")) return;

        meshRenderer.material.SetInt("_ZTest", (int)CompareFunction.LessEqual);
        meshTransform.localScale = initialScale;
    }
}

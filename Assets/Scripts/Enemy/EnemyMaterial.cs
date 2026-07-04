using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyMaterial : MonoBehaviour {
    public Renderer meshRenderer;

    void OnTriggerEnter(Collider collider) {
        if (!collider.CompareTag("Player")) return;

        meshRenderer.material.SetInt("_ZTest", (int)CompareFunction.Always);
    }

    void OnTriggerExit(Collider collider) {
        if (!collider.CompareTag("Player")) return;

        meshRenderer.material.SetInt("_ZTest", (int)CompareFunction.LessEqual);
    }
}

using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(VisualEffect))]
public class EffectHelper : MonoBehaviour {
    private VisualEffect visualEffect;

    void Start() {
        visualEffect = GetComponent<VisualEffect>();
    }

    public void PlayEffectAtPosition(Vector3 position, Quaternion rotation) {
        transform.parent = null;

        transform.position = position;
        transform.rotation = rotation;

        visualEffect.Play();
    }
}

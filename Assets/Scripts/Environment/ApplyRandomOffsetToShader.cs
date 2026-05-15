using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class ApplyRandomOffsetToShader : MonoBehaviour {
    void Start() {
        Material material = GetComponent<Renderer>().material;
        material.SetFloat("_Random_Offset", Random.Range(0f, 100f));
    }
}

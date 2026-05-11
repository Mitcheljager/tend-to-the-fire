using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteAlways]
public class VertexSnappingSettings : MonoBehaviour {
    [Header("Shaders")]
    public List<Shader> shaders = new();

    [Header("Settings")]
    public bool enableVertexSnapping = true;
    [Range(0, 128)]
    public int vertexSnapSize = 32;
    [Range(0, 5)]
    public float vertexSnapDistance = 3;

    [Header("State")]
    public List<Material> materials = new();

    private static readonly int enableVertexSnappingID = Shader.PropertyToID("_Enable_Vertex_Snapping");
    private static readonly int vertexSnapSizeID = Shader.PropertyToID("_Vertex_Snap_Size");
    private static readonly int vertexSnapDistanceID = Shader.PropertyToID("_Vertex_Snap_Distance");

    void OnValidate() {
        if (shaders.Count > 0) GetAllMaterials();

        ApplySettings();
    }

    void OnEnable() {
        ApplySettings();
    }

    private void GetAllMaterials() {
        HashSet<Material> foundMaterials = new();

        foreach(Shader shader in shaders) {
            foreach (Renderer renderer in FindObjectsByType<Renderer>(FindObjectsSortMode.None)){
                foreach (Material material in renderer.sharedMaterials) {
                    if (material != null && material.shader == shader) foundMaterials.Add(material);
                }
            }
        }

        materials = foundMaterials.ToList();
    }

    private void ApplySettings() {
        foreach (Material material in materials) {
            material.SetFloat(enableVertexSnappingID, enableVertexSnapping ? 1f : 0f);
            material.SetInt(vertexSnapSizeID, vertexSnapSize);
            material.SetFloat(vertexSnapDistanceID, vertexSnapDistance);
        }
    }
}

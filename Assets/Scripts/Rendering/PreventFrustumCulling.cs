// https://www.vertexfragment.com/ramblings/unity-prevent-object-culling/

using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public sealed class PreventFrustumCulling : MonoBehaviour {
    private MeshRenderer Renderer;

    private void Start() {
        Renderer = GetComponent<MeshRenderer>();
    }

    private void Update() {
        if ((Camera.main == null) || (Renderer == null)) return;

        Bounds adjustedBounds = Renderer.bounds;
        adjustedBounds.center = Camera.main.transform.position + ((Camera.main.farClipPlane - Camera.main.nearClipPlane) * 0.5f * Camera.main.transform.forward);
        adjustedBounds.extents = new Vector3(0.1f, 0.1f, 0.1f);

        Renderer.bounds = adjustedBounds;
    }
}

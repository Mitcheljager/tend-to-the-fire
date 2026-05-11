using UnityEngine;

public class BoxColliderGizmos : MonoBehaviour {
    public Color outlineColor = Color.magenta;
    public Color faceColor = new(1f, 0f, 1f, 0.15f);

    private void OnDrawGizmos() {
        BoxCollider[] colliders = GetComponents<BoxCollider>();

        foreach (var collider in colliders) {
            if (!collider.enabled) continue;

            Gizmos.color = faceColor;

            Matrix4x4 oldMatrix = Gizmos.matrix;

            Gizmos.matrix = Matrix4x4.TRS(collider.transform.position, collider.transform.rotation, collider.transform.lossyScale);

            Gizmos.DrawCube(collider.center, collider.size);

            Gizmos.color = outlineColor;
            Gizmos.DrawWireCube(collider.center, collider.size);

            Gizmos.matrix = oldMatrix;
        }
    }
}

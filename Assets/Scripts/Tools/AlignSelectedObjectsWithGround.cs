using UnityEngine;
using UnityEditor;

public class AlignSelectedObjectsWithGround : EditorWindow {
    private float offset = 0f;
    private LayerMask placementMask = 1 << 3;

    [MenuItem("Tools/Align selected objects with ground")]
    public static void ShowWindow() {
        GetWindow<AlignSelectedObjectsWithGround>("Align selected objects with ground");
    }

    private void OnGUI() {
        offset = EditorGUILayout.FloatField("Offset", offset);

        LayerMask mask = EditorGUILayout.MaskField(
            "Placement Layers",
            UnityEditorInternal.InternalEditorUtility.LayerMaskToConcatenatedLayersMask(placementMask),
            UnityEditorInternal.InternalEditorUtility.layers
        );

        placementMask = UnityEditorInternal.InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(mask);

        if (GUILayout.Button("Align", GUILayout.Height(30))) RandomizePositions();
    }

    private void RandomizePositions() {
        GameObject[] selectedObjects = Selection.gameObjects;

        Undo.RecordObjects(System.Array.ConvertAll(selectedObjects, selectedObject => (Object)selectedObject.transform), "Align");

        foreach (GameObject selectedObject in selectedObjects) {
            Vector3 currentPosition = selectedObject.transform.position;

            if (!Physics.Raycast(currentPosition + Vector3.up * 10f, Vector3.down, out RaycastHit hit, 20f, placementMask)) return;

            selectedObject.transform.position = new Vector3(currentPosition.x, hit.point.y + offset, currentPosition.z);
        }
    }
}

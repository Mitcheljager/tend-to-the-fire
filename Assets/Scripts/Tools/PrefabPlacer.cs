using UnityEngine;
using UnityEditor;

public class PrefabPlacer : EditorWindow {
    private GameObject selectedPrefab;
    private bool isPlacing = false;
    private bool alignToSurface = true;
    private float offset = 0f;
    private LayerMask placementMask = (1 << 0) | (1 << 3);

    private GameObject previewInstance;

    [MenuItem("Tools/Prefab placer")]
    public static void ShowWindow() {
        GetWindow<PrefabPlacer>("Prefab placer");
    }

    private void OnEnable() {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable() {
        SceneView.duringSceneGui -= OnSceneGUI;

        StopPlacing();
    }

    private void OnGUI() {
        EditorGUI.BeginChangeCheck();

        selectedPrefab = (GameObject)EditorGUILayout.ObjectField(
            "Prefab", selectedPrefab, typeof(GameObject), false
        );

        if (EditorGUI.EndChangeCheck()) RefreshPreview();

        alignToSurface = EditorGUILayout.Toggle("Align to surface normal", alignToSurface);
        offset = EditorGUILayout.FloatField("Offset", offset);

        LayerMask mask = EditorGUILayout.MaskField(
            "Placement Layers",
            UnityEditorInternal.InternalEditorUtility.LayerMaskToConcatenatedLayersMask(placementMask),
            UnityEditorInternal.InternalEditorUtility.layers
        );

        placementMask = UnityEditorInternal.InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(mask);

        Color originalColor = GUI.backgroundColor;

        if (!isPlacing) {
            GUI.backgroundColor = new Color(0.4f, 0.8f, 0.4f);

            if (GUILayout.Button("Start placing", GUILayout.Height(32))) StartPlacing();
        } else {
            GUI.backgroundColor = new Color(0.9f, 0.4f, 0.4f);

            if (GUILayout.Button("Stop placing", GUILayout.Height(32))) StopPlacing();
        }

        GUI.backgroundColor = originalColor;
        GUI.enabled = true;

        EditorGUILayout.Space();
    }

    private void StartPlacing() {
        if (selectedPrefab == null) return;

        isPlacing = true;

        CreatePreview();
        Repaint();
    }

    private void StopPlacing() {
        isPlacing = false;

        DestroyPreview();
        Repaint();
    }

    private void CreatePreview() {
        DestroyPreview();

        if (selectedPrefab == null) return;

        previewInstance = (GameObject)PrefabUtility.InstantiatePrefab(selectedPrefab);
        previewInstance.name = "__PrefabPlacerPreview__";
        previewInstance.hideFlags = HideFlags.HideAndDontSave;

        SetPreviewInteractable(previewInstance, false);
    }

    private void DestroyPreview() {
        if (previewInstance != null) {
            DestroyImmediate(previewInstance);

            previewInstance = null;
        }
    }

    private void RefreshPreview() {
        if (isPlacing) CreatePreview();
    }

    private void SetPreviewInteractable(GameObject gameObject, bool interactable) {
        foreach (Collider collider in gameObject.GetComponentsInChildren<Collider>()) {
            collider.enabled = interactable;
        }
    }

    private void OnSceneGUI(SceneView sceneView) {
        if (!isPlacing || selectedPrefab == null) return;

        Event mouseEvent = Event.current;

        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

        Ray ray = HandleUtility.GUIPointToWorldRay(mouseEvent.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit, 1000f, placementMask)) return;

        Vector3 position = hit.point + hit.normal * offset;
        Quaternion rotation = alignToSurface ? Quaternion.FromToRotation(Vector3.up, hit.normal) : Quaternion.identity;

        if (previewInstance != null) {
            previewInstance.transform.SetPositionAndRotation(position, rotation);
        }

        Handles.color = new Color(0.2f, 0.9f, 0.2f, 0.8f);
        Handles.DrawWireDisc(hit.point, hit.normal, 0.5f);
        Handles.DrawLine(hit.point, hit.point + hit.normal * 0.75f);

        if (mouseEvent.type == EventType.MouseDown && mouseEvent.button == 0) {
            PlacePrefab(position, rotation);

            mouseEvent.Use();
        }

        sceneView.Repaint();
    }

    private void PlacePrefab(Vector3 position, Quaternion rotation) {
        if (selectedPrefab == null) return;

        GameObject placed = (GameObject)PrefabUtility.InstantiatePrefab(selectedPrefab);
        placed.transform.SetPositionAndRotation(position, rotation);

        GameObjectUtility.EnsureUniqueNameForSibling(placed);

        Undo.RegisterCreatedObjectUndo(placed, $"Place {selectedPrefab.name}");
    }
}

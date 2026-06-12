using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class PrefabPlacer : EditorWindow {
    private GameObject selectedPrefab;
    private bool isPlacing = false;
    private bool alignToSurface = true;
    public bool randomRotationY = true;
    public int numberOfObjects = 1;
    private float offset = 0f;
    private Vector3 randomPositionWithinRange = new();
    private LayerMask placementMask = (1 << 0) | (1 << 3);

    private List<GameObject> previewInstances;

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
        randomRotationY = EditorGUILayout.Toggle("Random rotation y", randomRotationY);
        numberOfObjects = EditorGUILayout.IntField("Number of objects", numberOfObjects);
        offset = EditorGUILayout.FloatField("Offset", offset);
        randomPositionWithinRange = EditorGUILayout.Vector3Field("Random position within range", randomPositionWithinRange);

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

        for (int index = 0; index < numberOfObjects; index++) {
            previewInstances.Add((GameObject)PrefabUtility.InstantiatePrefab(selectedPrefab));
            previewInstances.Last().name = "__PrefabPlacerPreview__";
            previewInstances.Last().hideFlags = HideFlags.HideAndDontSave;

            SetPreviewInteractable(previewInstances.Last(), false);
        }
    }

    private void DestroyPreview() {
        while(previewInstances.Count > 0) {
            DestroyImmediate(previewInstances.Last());
            previewInstances.Remove(previewInstances.Last());
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
        if (!isPlacing || selectedPrefab == null || Event.current.alt) return;

        Event mouseEvent = Event.current;

        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

        Ray ray = HandleUtility.GUIPointToWorldRay(mouseEvent.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit cursorHit, 1000f, placementMask)) return;

        int index = 0;
        foreach (GameObject previewInstance in previewInstances) {
            Random.InitState((int)(cursorHit.point.magnitude * 1000 + index));

            Vector3 randomPositionOffset = new(
                Random.Range(-randomPositionWithinRange.x, randomPositionWithinRange.x),
                Random.Range(-randomPositionWithinRange.y, randomPositionWithinRange.y),
                Random.Range(-randomPositionWithinRange.z, randomPositionWithinRange.z)
            );

            float randomYRotation = Random.Range(0f, 360f);

            if (!Physics.Raycast(cursorHit.point + randomPositionOffset + Vector3.up * 20f, Vector3.down, out RaycastHit hit, 50f, placementMask)) return;

            Vector3 position = hit.point + hit.normal * offset;
            Quaternion rotation = alignToSurface ? Quaternion.FromToRotation(Vector3.up, hit.normal) : Quaternion.identity;

            previewInstance.transform.position = position;
            previewInstance.transform.rotation = randomRotationY ? rotation * Quaternion.Euler(0f, randomYRotation, 0f) : rotation;

            index++;
        }

        Handles.color = new Color(0.2f, 0.9f, 0.2f, 0.8f);
        Handles.DrawWireDisc(cursorHit.point, cursorHit.normal, Mathf.Max(0.5f, randomPositionWithinRange.x, randomPositionWithinRange.z));
        Handles.DrawLine(cursorHit.point, cursorHit.point + cursorHit.normal * 0.75f);

        if (mouseEvent.type == EventType.MouseDown && mouseEvent.button == 0) {
            PlacePrefabs();

            mouseEvent.Use();
        }

        sceneView.Repaint();
    }

    private void PlacePrefabs() {
        foreach (GameObject previewInstance in previewInstances) {
            GameObject placed = (GameObject)PrefabUtility.InstantiatePrefab(selectedPrefab);

            Debug.Log(previewInstance.transform.position);

            placed.transform.position = previewInstance.transform.position;
            placed.transform.rotation = previewInstance.transform.rotation;

            GameObjectUtility.EnsureUniqueNameForSibling(placed);

            Undo.RegisterCreatedObjectUndo(placed, $"Place {selectedPrefab.name}");
        }
    }
}

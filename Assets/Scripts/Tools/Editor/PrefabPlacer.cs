using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class PrefabPlacer : EditorWindow {
    public List<GameObject> possiblePrefabs = new();
    private Transform transformParent = null;
    private bool isPlacing = false;
    private bool alignToSurface = true;
    private bool randomRotationY = true;
    private int numberOfObjects = 1;
    private float offset = 0f;
    private Vector3 randomPositionWithinRange = new();
    private LayerMask placementMask = 1 << 3;

    private List<GameObject> previewInstances = new();
    private List<GameObject> previewSourcePrefabs = new();

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
        SerializedObject serializedObject = new(this);
        SerializedProperty prefabsProp = serializedObject.FindProperty("possiblePrefabs");

        EditorGUILayout.PropertyField(prefabsProp, new GUIContent("Prefabs"), true);
        serializedObject.ApplyModifiedProperties();

        if (EditorGUI.EndChangeCheck()) RefreshPreview();

        transformParent = EditorGUILayout.ObjectField("Transform parent", transformParent, typeof(Transform), true) as Transform;
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
        if (possiblePrefabs.Count == 0) return;

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

        if (possiblePrefabs.Count == 0) return;

        previewInstances = new List<GameObject>();
        previewSourcePrefabs = new List<GameObject>();

        for (int index = 0; index < numberOfObjects; index++) {
            GameObject selectedPrefab = possiblePrefabs[Random.Range(0, possiblePrefabs.Count)];

            previewSourcePrefabs.Add(selectedPrefab);

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
        if (!isPlacing || possiblePrefabs.Count == 0 || Event.current.alt) return;

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
        for (int i = 0; i < previewInstances.Count; i++) {
            GameObject previewInstance = previewInstances[i];
            GameObject placed = (GameObject)PrefabUtility.InstantiatePrefab(previewSourcePrefabs[i]);

            placed.transform.position = previewInstance.transform.position;
            placed.transform.rotation = previewInstance.transform.rotation;

            if (transformParent != null) placed.transform.parent = transformParent;

            GameObjectUtility.EnsureUniqueNameForSibling(placed);

            Undo.RegisterCreatedObjectUndo(placed, $"Place {placed.name}");
        }
    }
}

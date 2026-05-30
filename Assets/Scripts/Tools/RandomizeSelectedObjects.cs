using UnityEngine;
using UnityEditor;

public class RandomizeSelectedObjects : EditorWindow {
    private float randomRange = 5f;
    private bool x = true;
    private bool y = false;
    private bool z = true;

    [MenuItem("Tools/Randomize object positions")]
    public static void ShowWindow() {
        GetWindow<RandomizeSelectedObjects>("Randomize positions");
    }

    private void OnGUI() {
        randomRange = EditorGUILayout.FloatField("Range", randomRange);

        x = EditorGUILayout.Toggle("X", x);
        y = EditorGUILayout.Toggle("Y", y);
        z = EditorGUILayout.Toggle("Z", z);

        if (GUILayout.Button("Randomize positions", GUILayout.Height(30))) RandomizePositions();
    }

    private void RandomizePositions() {
        GameObject[] selectedObjects = Selection.gameObjects;

        Undo.RecordObjects(System.Array.ConvertAll(selectedObjects, selectedObject => (Object)selectedObject.transform), "Randomize positions");

        foreach (GameObject selectedObject in selectedObjects) {
            Vector3 currentPosition = selectedObject.transform.position;

            float newX = currentPosition.x + (x ? Random.Range(-randomRange, randomRange) : 0f);
            float newY = currentPosition.y + (y ? Random.Range(-randomRange, randomRange) : 0f);
            float newZ = currentPosition.z + (z ? Random.Range(-randomRange, randomRange) : 0f);

            selectedObject.transform.position = new Vector3(newX, newY, newZ);
        }
    }
}

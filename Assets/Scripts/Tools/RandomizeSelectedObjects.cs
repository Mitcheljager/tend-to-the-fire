using UnityEngine;
using UnityEditor;

public class RandomizeSelectedObjects : EditorWindow {
    private float randomRange = 5f;
    private bool positionX = true;
    private bool positionY = false;
    private bool positionZ = true;
    private bool rotationX = false;
    private bool rotationY = false;
    private bool rotationZ = false;

    [MenuItem("Tools/Randomize object positions")]
    public static void ShowWindow() {
        GetWindow<RandomizeSelectedObjects>("Randomize positions");
    }

    private void OnGUI() {
        randomRange = EditorGUILayout.FloatField("Range", randomRange);

        positionX = EditorGUILayout.Toggle("X Position", positionX);
        positionY = EditorGUILayout.Toggle("Y Position", positionY);
        positionZ = EditorGUILayout.Toggle("Z Position", positionZ);

        EditorGUILayout.Space();

        rotationX = EditorGUILayout.Toggle("X Rotation", rotationX);
        rotationY = EditorGUILayout.Toggle("Y Rotation", rotationY);
        rotationZ = EditorGUILayout.Toggle("Z Rotation", rotationZ);

        if (GUILayout.Button("Randomize positions", GUILayout.Height(30))) RandomizePositions();
    }

    private void RandomizePositions() {
        GameObject[] selectedObjects = Selection.gameObjects;

        Undo.RecordObjects(System.Array.ConvertAll(selectedObjects, selectedObject => (Object)selectedObject.transform), "Randomize positions");

        foreach (GameObject selectedObject in selectedObjects) {
            Vector3 currentPosition = selectedObject.transform.position;
            float newPositionX = currentPosition.x + (positionX ? Random.Range(-randomRange, randomRange) : 0f);
            float newPositionY = currentPosition.y + (positionY ? Random.Range(-randomRange, randomRange) : 0f);
            float newPositionZ = currentPosition.z + (positionZ ? Random.Range(-randomRange, randomRange) : 0f);

            Vector3 currentRotation = selectedObject.transform.eulerAngles;
            float newRotationX = rotationX ? Random.Range(0f, 360f) : 0f;
            float newRotationY = rotationY ? Random.Range(0f, 360f) : 0f;
            float newRotationZ = rotationZ ? Random.Range(0f, 360f) : 0f;

            selectedObject.transform.position = new(newPositionX, newPositionY, newPositionZ);
            selectedObject.transform.rotation = Quaternion.Euler(newRotationX, newRotationY, newRotationZ);
        }
    }
}

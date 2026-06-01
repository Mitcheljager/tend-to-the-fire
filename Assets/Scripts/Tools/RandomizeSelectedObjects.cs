using UnityEngine;
using UnityEditor;

public class RandomizeSelectedObjects : EditorWindow {
    private float positionRange = 5f;
    private bool positionX = true;
    private bool positionY = false;
    private bool positionZ = true;
    private bool rotationX = false;
    private bool rotationY = false;
    private bool rotationZ = false;
    private Vector2 scaleBetween = new(1, 1);

    [MenuItem("Tools/Randomize object positions")]
    public static void ShowWindow() {
        GetWindow<RandomizeSelectedObjects>("Randomize positions");
    }

    private void OnGUI() {
        positionRange = EditorGUILayout.FloatField("Position range", positionRange);

        positionX = EditorGUILayout.Toggle("X Position", positionX);
        positionY = EditorGUILayout.Toggle("Y Position", positionY);
        positionZ = EditorGUILayout.Toggle("Z Position", positionZ);

        EditorGUILayout.Space();

        rotationX = EditorGUILayout.Toggle("X Rotation", rotationX);
        rotationY = EditorGUILayout.Toggle("Y Rotation", rotationY);
        rotationZ = EditorGUILayout.Toggle("Z Rotation", rotationZ);

        scaleBetween = EditorGUILayout.Vector2Field("Scale between", scaleBetween);

        if (GUILayout.Button("Randomize", GUILayout.Height(30))) RandomizePositions();
    }

    private void RandomizePositions() {
        GameObject[] selectedObjects = Selection.gameObjects;

        Undo.RecordObjects(System.Array.ConvertAll(selectedObjects, selectedObject => (Object)selectedObject.transform), "Randomize positions");

        foreach (GameObject selectedObject in selectedObjects) {
            Vector3 currentPosition = selectedObject.transform.position;
            float newPositionX = currentPosition.x + (positionX ? Random.Range(-positionRange, positionRange) : 0f);
            float newPositionY = currentPosition.y + (positionY ? Random.Range(-positionRange, positionRange) : 0f);
            float newPositionZ = currentPosition.z + (positionZ ? Random.Range(-positionRange, positionRange) : 0f);

            Vector3 currentRotation = selectedObject.transform.eulerAngles;
            float newRotationX = rotationX ? Random.Range(0f, 360f) : currentRotation.x;
            float newRotationY = rotationY ? Random.Range(0f, 360f) : currentRotation.y;
            float newRotationZ = rotationZ ? Random.Range(0f, 360f) : currentRotation.z;

            float randomScale = Random.Range(scaleBetween.x, scaleBetween.y);

            selectedObject.transform.position = new(newPositionX, newPositionY, newPositionZ);
            selectedObject.transform.rotation = Quaternion.Euler(newRotationX, newRotationY, newRotationZ);
            selectedObject.transform.localScale = Vector3.one * randomScale;
        }
    }
}

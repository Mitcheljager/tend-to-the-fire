using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Fire))]
public class FireEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        Fire fire = (Fire)target;

        if (EditorApplication.isPlaying && GUILayout.Button("Add max fuel")) {
            fire.AddEditorFuel(fire.maxFuel);
        }

        if (EditorApplication.isPlaying && GUILayout.Button("Add 5 fuel")) {
            fire.AddEditorFuel(5);
        }

        EditorGUILayout.LabelField("Fuel progress", EditorStyles.boldLabel);

        foreach (Fuel fuel in fire.activeFuel) {
            EditorGUILayout.Slider(fuel.gameObject.name, fuel.currentFuelNormalized, 0f, 1f);
        }

        if (fire.activeFuel.Count == 0) {
            EditorGUILayout.LabelField("No active fuel");
        }
    }
}

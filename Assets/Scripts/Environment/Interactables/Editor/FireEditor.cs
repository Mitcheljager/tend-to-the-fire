using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Fire))]
public class FireEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        Fire fire = (Fire)target;

        if (EditorApplication.isPlaying && GUILayout.Button("Add max fuel")) {
            GameObject fuelObject = new("Editor created fuel");
            fuelObject.AddComponent<Fuel>();
            Fuel fuel = fuelObject.GetComponent<Fuel>();
            fuel.maxFuel = fire.maxFuel;

            fire.activeFuel.Add(fuel);
        }

        if (EditorApplication.isPlaying && GUILayout.Button("Add 5 fuel")) {
            GameObject fuelObject = new("Editor created fuel");
            fuelObject.AddComponent<Fuel>();
            Fuel fuel = fuelObject.GetComponent<Fuel>();
            fuel.maxFuel = 5f;

            fire.activeFuel.Add(fuel);
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

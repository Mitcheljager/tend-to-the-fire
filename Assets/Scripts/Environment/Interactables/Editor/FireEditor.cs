using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Fire))]
public class FireEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        Fire fire = (Fire)target;

        if (EditorApplication.isPlaying && GUILayout.Button("Add fuel")) {
            GameObject fuelObject = new("Editor created fuel");
            fuelObject.AddComponent<Fuel>();
            Fuel fuel = fuelObject.GetComponent<Fuel>();
            fuel.maxFuel = fire.maxFuel;

            Debug.Log(fire.maxFuel);

            fire.activeFuel.Add(fuel);
        }
    }
}

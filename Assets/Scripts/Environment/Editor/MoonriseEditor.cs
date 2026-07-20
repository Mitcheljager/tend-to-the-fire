using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Moonrise))]
public class MoonriseEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        Moonrise enemy = (Moonrise)target;

        if (EditorApplication.isPlaying && GUILayout.Button("Restart")) {
            enemy.RestartMoonrise();
        }
    }
}

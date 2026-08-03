using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FireSmother))]
public class FireSmotherEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        FireSmother fireSmother = (FireSmother)target;

        if (EditorApplication.isPlaying && GUILayout.Button("Reset smother")) {
            fireSmother.currentSmother = 0f;
        }
    }
}

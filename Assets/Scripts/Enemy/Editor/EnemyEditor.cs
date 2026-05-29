using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Enemy))]
public class EnemyEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        Enemy enemy = (Enemy)target;

        if (EditorApplication.isPlaying && GUILayout.Button("Reposition")) {
            enemy.Reposition();
        }

        if (EditorApplication.isPlaying && GUILayout.Button("Despawn")) {
            enemy.Despawn();
        }
    }
}

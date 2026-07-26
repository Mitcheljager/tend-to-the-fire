using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyNavigation))]
public class EnemyNavigationEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        EnemyNavigation enemyNavigation = (EnemyNavigation)target;

        if (EditorApplication.isPlaying && GUILayout.Button("Set random destination")) {
            enemyNavigation.SetRandomValidDestination();
        }
    }
}

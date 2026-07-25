using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyNavigation))]
public class EnemyNavigationEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        EnemyNavigation enemyNavigation = (EnemyNavigation)target;

        if (EditorApplication.isPlaying && GUILayout.Button("Set random destination")) {
            EnemyManager enemyManager = FindFirstObjectByType<EnemyManager>();
            Vector3? position = enemyManager.FindValidPosition();

            if (position == null) return;

            enemyNavigation.SetDestination(position.Value);
        }
    }
}

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyNavigation))]
public class EnemyNavigationEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        EnemyNavigation enemyNavigation = (EnemyNavigation)target;

        GUILayout.Label("isStopped: " + enemyNavigation.agent.isStopped);
        GUILayout.Label("remainingDistance: " + enemyNavigation.agent.remainingDistance);

        if (EditorApplication.isPlaying && GUILayout.Button("Set random destination")) {
            enemyNavigation.SetRandomValidDestination();
        }
    }
}

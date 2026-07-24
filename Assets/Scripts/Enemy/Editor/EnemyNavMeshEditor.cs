using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyNavMesh))]
public class EnemyNavMeshEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        EnemyNavMesh enemyNavMesh = (EnemyNavMesh)target;

        if (EditorApplication.isPlaying && GUILayout.Button("Set random destination")) {
            EnemyManager enemyManager = FindFirstObjectByType<EnemyManager>();
            Vector3? position = enemyManager.FindValidPosition();

            if (position == null) return;

            enemyNavMesh.SetDestination(position.Value);
        }
    }
}

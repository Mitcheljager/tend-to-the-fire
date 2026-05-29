using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyManager))]
public class EnemyManagerEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        EnemyManager enemyManager = (EnemyManager)target;

        if (EditorApplication.isPlaying && GUILayout.Button("Despawn all")) {
            enemyManager.DespawnAllEnemies();
        }
    }
}

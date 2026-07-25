using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyFindPlayer))]
public class EnemyFindPlayerEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        EnemyFindPlayer enemyFindPlayer = (EnemyFindPlayer)target;

        if (EditorApplication.isPlaying) GUILayout.Label("Player Seen: " + enemyFindPlayer.IsPlayerSeen(), EditorStyles.boldLabel);
    }
}

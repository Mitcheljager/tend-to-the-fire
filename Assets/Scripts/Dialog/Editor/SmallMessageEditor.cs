using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SmallMessage))]
public class SmallMessageEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        SmallMessage smallMessage = (SmallMessage)target;

        if (EditorApplication.isPlaying && GUILayout.Button("Show small message")) {
            MessageEvent.ShowSmallMessage("I am a small message: " + smallMessage.messageQueue.Count);
        }
    }
}

using UnityEngine;
using UnityEditor;
using System.IO;

public class AssetTypeViewer : EditorWindow {
    private Vector2 scrollPosition = Vector2.zero;

    [MenuItem("Tools/Asset type viewer")]
    public static void ShowWindow() {
        GetWindow<AssetTypeViewer>("Asset type viewer");
    }

    private void OnGUI() {
        if (Application.isPlaying) return;
        if (!EditorApplication.isFocused) return;

        scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true);

        string[] directories = Directory.GetDirectories("Assets\\Prefabs", "*", SearchOption.AllDirectories);

        foreach(string directory in directories) {
            GUIStyle directoryLabelStyle = new GUIStyle() { normal = new GUIStyleState() { textColor = Color.white } };
            directoryLabelStyle.fontSize = 21;
            EditorGUILayout.LabelField(directory, directoryLabelStyle);

            string[] files = Directory.GetFiles(directory, "*.prefab");

            foreach(string file in files) {
                EditorGUILayout.LabelField(file);

                Object prefab = null;
                prefab = AssetDatabase.LoadMainAssetAtPath(file);

                GUILayout.Box(AssetPreview.GetAssetPreview(prefab), GUILayout.Height(50), GUILayout.Width(50));
            }
        }

        GUILayout.EndScrollView();
    }
}

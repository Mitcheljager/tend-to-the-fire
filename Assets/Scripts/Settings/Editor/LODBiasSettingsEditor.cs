using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LODBiasSettings))]
public class LODBiasSettingsEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        LODBiasSettings lodSettings = (LODBiasSettings)target;

        if (GUILayout.Button("Set Low")) {
            Settings.SetSettingInt(SettingsKey.LODBias, 0);
            lodSettings.ApplySettings();
        }

        if (GUILayout.Button("Set Default")) {
            Settings.SetSettingInt(SettingsKey.LODBias, 1);
            lodSettings.ApplySettings();
        }

        if (GUILayout.Button("Set High")) {
            Settings.SetSettingInt(SettingsKey.LODBias, 2);
            lodSettings.ApplySettings();
        }
    }
}

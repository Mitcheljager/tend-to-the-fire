using UnityEngine;

public class WindowSettings : UserSettings {
    void Start() {
        QualitySettings.vSyncCount = 0;

        SetMaxFPS();
        SetVSyncCount();
    }

    public override void PossibilyUpdateFromEvent(SettingsKey key) {
        if (key == SettingsKey.MaxFPS) SetMaxFPS();
        if (key == SettingsKey.VSync) SetVSyncCount();
    }

    public void SetMaxFPS() {
        int maxFPS = Settings.GetSettingInt(SettingsKey.MaxFPS, 120);

        Application.targetFrameRate = Mathf.Clamp(maxFPS, 30, 300);
    }

    public void SetVSyncCount() {
        QualitySettings.vSyncCount = Settings.IsSettingBoolEnabled(SettingsKey.VSync) ? 1 : 0;
    }
}

using UnityEngine;

public class ControlsSettings : UserSettings {
    private PlayerCamera playerCamera;

    void OnValidate() {
        ApplySettings();
    }

    void Start() {
        ApplySettings();
    }

    public override void PossibilyUpdateFromEvent(SettingsKey key) {
        if (key != SettingsKey.MouseSensitivity && key != SettingsKey.ControllerSensitivity) return;

        ApplySettings();
    }

    public void ApplySettings() {
        if (playerCamera == null) playerCamera = FindFirstObjectByType<PlayerCamera>();

        playerCamera.mouseSensitivity = Mathf.Max(Settings.GetSettingInt(SettingsKey.MouseSensitivity, 25) / 10f, 0.01f);
        playerCamera.controllerSensitivity = Mathf.Max(Settings.GetSettingInt(SettingsKey.ControllerSensitivity, 50) * 2f, 0.01f);
    }
}

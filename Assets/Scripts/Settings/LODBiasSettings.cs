using UnityEngine;

public class LODBiasSettings : UserSettings {
    void OnValidate() {
        ApplySettings();
    }

    void Start() {
        ApplySettings();
    }

    public override void PossibilyUpdateFromEvent(SettingsKey key) {
        if (key != SettingsKey.LODBias) return;

        ApplySettings();
    }

    public void ApplySettings() {
        int storedValue = Settings.GetSettingInt(SettingsKey.LODBias, 1);

        if (storedValue == 0) QualitySettings.lodBias = 1.25f;
        else if (storedValue == 1) QualitySettings.lodBias = 2;
        else if (storedValue == 2) QualitySettings.lodBias = 3;
    }
}

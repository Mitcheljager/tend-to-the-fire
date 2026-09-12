using UnityEngine;

public class LODBiasSettings : MonoBehaviour {
    void OnValidate() {
        ApplySettings();
    }

    void OnEnable() {
        ApplySettings();
    }

    public void ApplySettings() {
        int storedValue = Settings.GetSettingInt(SettingsKey.LODBias);

        if (storedValue == 0) QualitySettings.lodBias = 1.25f;
        else if (storedValue == 1) QualitySettings.lodBias = 2;
        else if (storedValue == 2) QualitySettings.lodBias = 3;
    }
}

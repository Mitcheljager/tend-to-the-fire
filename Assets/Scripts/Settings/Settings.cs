using UnityEngine;

public enum SettingsKey {
    Bloom,
    PostExposure,
    Shadows,
    MasterVolume,
    SoundEffectsVolume,
    AmbienceVolume,
    MusicVolume,
    RenderScale,
    MaxFPS,
    VSync,
    LODBias,
    SimpleLighting,
    MouseSensitivity,
    ControllerSensitivity
}

public static class Settings {
    public static bool IsSettingBoolEnabled(SettingsKey key) {
        return GetSettingInt(key, 0) > 0;
    }

    public static void ToggleSettingBool(SettingsKey key, bool value) {
        PlayerPrefs.SetInt(key.ToString(), value ? 1 : 0);
    }

    public static int GetSettingInt(SettingsKey key, int defaultValue = 1) {
        return PlayerPrefs.GetInt(key.ToString(), defaultValue);
    }

    public static void SetSettingInt(SettingsKey key, int value) {
        PlayerPrefs.SetInt(key.ToString(), value);
    }

    public static float GetSettingFloat(SettingsKey key, float defaultValue = 1f) {
        return PlayerPrefs.GetFloat(key.ToString(), defaultValue);
    }

    public static void SetSettingFloat(SettingsKey key, float value) {
        PlayerPrefs.SetFloat(key.ToString(), value);
    }
}

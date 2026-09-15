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
    VSyncCount,
    LODBias
}

public static class Settings {
    public static bool IsSettingBoolEnabled(SettingsKey key) {
        return GetSettingInt(key) > 0;
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
}

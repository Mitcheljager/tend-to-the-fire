using UnityEngine;

public enum SettingsKey {
    Bloom,
    PostExposure,
    Shadows,
    ScreenShake,
    MasterVolume,
    SoundEffectsVolume,
    AmbienceVolume,
    MusicVolume,
    RenderScale,
    FishEye,
    MaxFPS
}

public static class Settings {
    public static bool IsSettingEnabled(SettingsKey key) {
        return PlayerPrefs.GetInt(key.ToString(), 1) > 0;
    }

    public static void ToggleSetting(SettingsKey key, bool value) {
        PlayerPrefs.SetInt(key.ToString(), value ? 1 : 0);
    }
}

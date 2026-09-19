using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class AudioVolumeSettings : UserSettings {
    public AudioMixer mixer;

    private readonly SettingsKey[] volumeKeys = { SettingsKey.MasterVolume, SettingsKey.SoundEffectsVolume, SettingsKey.AmbienceVolume, SettingsKey.MusicVolume };

    void Start() {
        foreach (var key in volumeKeys) {
            SetVolume(key);
        }
    }

    override public void PossibilyUpdateFromEvent(SettingsKey key) {
        if (!volumeKeys.Contains(key)) return;

        SetVolume(key);
    }

    public void SetVolume(SettingsKey key) {
        float volume = Settings.GetSettingInt(key, 80) / 100f;
        if (volume == 0f) volume = 0.00001f;

        float log10volume = Mathf.Log10(volume) * 20;

        mixer.SetFloat(key.ToString(), log10volume);
    }
}

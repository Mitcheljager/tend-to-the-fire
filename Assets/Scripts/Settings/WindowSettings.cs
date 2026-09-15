using UnityEngine;

public class WindowSettings : MonoBehaviour {
    void Start() {
        QualitySettings.vSyncCount = 0;

        SetMaxFPS();
        SetVSyncCount();
    }

    void OnEnable() {
        ChangeEvent.OnChangeEvent.AddListener(PossibilyUpdateFromEvent);
    }

    void OnDisable() {
        ChangeEvent.OnChangeEvent.RemoveListener(PossibilyUpdateFromEvent);
    }

    private void PossibilyUpdateFromEvent(SettingsKey key) {
        if (key == SettingsKey.MaxFPS) SetMaxFPS();
        if (key == SettingsKey.VSyncCount) SetVSyncCount();
    }

    public void SetMaxFPS() {
        string key = SettingsKey.MaxFPS.ToString();
        int maxFPS = (int)PlayerPrefs.GetFloat(key, 300f);

        Application.targetFrameRate = Mathf.Clamp(maxFPS, 30, 500);
    }

    public void SetVSyncCount() {
        string key = SettingsKey.VSyncCount.ToString();
        QualitySettings.vSyncCount = PlayerPrefs.GetInt(key, 0);
    }
}

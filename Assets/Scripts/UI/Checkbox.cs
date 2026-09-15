using UnityEngine;

public class Checkbox : MonoBehaviour {
    public bool defaultValue = true;
    public SettingsKey key;
    public GameObject[] activeObjects;
    public AudioHelper audioHelperOnChange;

    private bool active = false;
    private bool initialValueSet = false;

    void OnEnable() {
        SetValue(Settings.IsSettingBoolEnabled(key, defaultValue));

        initialValueSet = true;
    }

    void OnDisable() {
        initialValueSet = false;
    }

    public void Change() {
        if (!initialValueSet) return;

        if (audioHelperOnChange != null) audioHelperOnChange.PlayRandomClip();

        ChangeEvent.Dispatch(key);
    }

    public void ToggleValue() {
        SetValue(!active);
    }

    public void SetValue(bool value) {
        Settings.ToggleSettingBool(key, value);

        active = value;

        foreach (GameObject activeObject in activeObjects) {
            activeObject.SetActive(active);
        }

        Change();
    }
}

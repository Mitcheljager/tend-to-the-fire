using UnityEngine;

public class RadioButtonGroup : MonoBehaviour {
    public SettingsKey key;
    public bool saveToUserSettings = true;
    public AudioHelper audioHelperOnChange;

    private RadioButton[] radioButtons;

    void Start() {
        radioButtons = GetComponentsInChildren<RadioButton>();

        SetValue(Settings.GetSettingInt(key, 0));
    }

    public void ToggleRadioButtons(int value) {
        SetValue(value);

        if (audioHelperOnChange != null) audioHelperOnChange.PlayRandomClip();

        if (saveToUserSettings) Settings.SetSettingInt(key, value);

        ChangeEvent.Dispatch(key);
    }

    private void SetValue(int value) {
        foreach(RadioButton radioButton in radioButtons) {
            radioButton.SetState(radioButton.value == value);
        }
    }
}

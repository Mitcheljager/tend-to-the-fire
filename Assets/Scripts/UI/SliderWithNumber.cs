using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderWithNumber : MonoBehaviour {
    public TextMeshProUGUI text;
    public Slider slider;
    public int defaultValue;
    public bool saveToUserSettings = true;
    public SettingsKey key;
    public AudioHelper audioHelperOnChange;

    private float value = 0;
    private bool initialValueSet = false;

    void OnEnable() {
        if (slider.wholeNumbers) value = Settings.GetSettingInt(key, defaultValue);
        else value = Settings.GetSettingFloat(key, defaultValue);

        text.text = value.ToString();
        slider.value = value;

        initialValueSet = true;
    }

    void OnDisable() {
        initialValueSet = false;
    }

    public void Change() {
        if (!initialValueSet) return;

        if (audioHelperOnChange != null && !audioHelperOnChange.audioSource.isPlaying) audioHelperOnChange.PlayRandomClip();

        ChangeEvent.Dispatch(key);
    }

    public void SetSliderValue() {
        value = Mathf.Round(slider.value * 100) / 100;

        if (slider.wholeNumbers) value = Mathf.Round(value);
        if (saveToUserSettings) {
            if (slider.wholeNumbers) Settings.SetSettingInt(key, (int)value);
            else Settings.SetSettingFloat(key, value);
        }

        slider.value = value;

        SetInputText(value);
        Change();
    }

    private void SetInputText(float value) {
        text.text = slider.wholeNumbers ?
            value.ToString() :
            string.Format("{0:F2}", value);
    }
}

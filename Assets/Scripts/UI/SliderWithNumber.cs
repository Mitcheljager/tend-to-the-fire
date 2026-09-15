using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderWithNumber : MonoBehaviour {
    public TextMeshProUGUI text;
    public Slider slider;
    public float defaultValue;
    public bool saveToUserSettings = true;
    public SettingsKey key;
    public AudioHelper audioHelperOnChange;

    private float value = 0f;
    private bool initialValueSet = false;

    void OnEnable() {
        value = PlayerPrefs.GetFloat(key.ToString(), defaultValue);

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
        if (saveToUserSettings) PlayerPrefs.SetFloat(key.ToString(), value);

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

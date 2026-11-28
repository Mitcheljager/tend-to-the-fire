using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using TMPro;

public class RebindInput : MonoBehaviour {
    public TMP_Text text;
    public SettingsKey key;
    public string defaultBinding;
    public bool allowMouse = true;

    private bool isAwaitingBinding = false;

    void OnDisable() {
        SetBinding(Settings.GetSettingString(key, defaultBinding));
    }

    void Start() {
        SetBinding(Settings.GetSettingString(key, defaultBinding));
    }

    void Update() {
        if (!isAwaitingBinding) return;
        if (new Controls().keyboard == null) return;

        foreach (KeyControl key in new Controls().keyboard.allKeys) {
            if ((key == null || !key.wasPressedThisFrame)) continue;

            SetBinding(key.name);
            break;
        }

        if (!allowMouse) return;

        foreach (ButtonControl control in new Controls().mouseControls) {
            if ((control == null || !control.wasPressedThisFrame)) continue;

            SetBinding(control.name);
            break;
        }
    }

    public void AwaitBinding() {
        if (isAwaitingBinding) return;

        text.text = "...";

        isAwaitingBinding = true;
    }

    public void SetBinding(string binding) {
        Settings.SetSettingString(key, binding);

        text.text = new Controls().getButtonDisplayOverride(binding);

        isAwaitingBinding = false;

        ChangeEvent.Dispatch(key);
    }
}

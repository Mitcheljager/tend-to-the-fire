using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class RebindInput : MonoBehaviour {
    public TMP_Text text;
    public SettingsKey key;
    public string defaultBinding;

    private bool isAwaitingBinding = false;

    void OnDisable() {
        SetBinding(Settings.GetSettingString(key, defaultBinding));
    }

    void Start() {
        SetBinding(Settings.GetSettingString(key, defaultBinding));
    }

    void Update() {
        if (!isAwaitingBinding) return;
        if (Keyboard.current == null) return;

        foreach (var key in Keyboard.current.allKeys) {
            if (key == null || !key.wasPressedThisFrame) continue;

            SetBinding(key.displayName.ToLower());
            break;
        }
    }

    public void AwaitBinding() {
        text.text = "...";

        isAwaitingBinding = true;
    }

    public void SetBinding(string binding) {
        Settings.SetSettingString(key, binding);

        text.text = binding;

        isAwaitingBinding = false;

        ChangeEvent.Dispatch(key);
    }
}

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using System.Collections.Generic;
using System.Linq;

public class Controls {
    public Gamepad gamepad = Gamepad.current;
    public Keyboard keyboard = Keyboard.current;
    public Mouse mouse = Mouse.current;

    public ButtonControl[] mouseControls = { Mouse.current.leftButton, Mouse.current.rightButton, Mouse.current.forwardButton, Mouse.current.backButton, Mouse.current.middleButton };

    public string getButtonDisplayOverride(string name) {
        Dictionary<string, string> buttonDisplayOverrides = new();

        buttonDisplayOverrides.Add("leftButton",    "Left mouse");
        buttonDisplayOverrides.Add("rightButton",   "Right mouse");
        buttonDisplayOverrides.Add("middleButton",  "Middle mouse");
        buttonDisplayOverrides.Add("forwardButton", "Forward mouse");
        buttonDisplayOverrides.Add("backButton",    "Back mouse");
        buttonDisplayOverrides.Add("leftShift",     "Left shift");
        buttonDisplayOverrides.Add("rightShift",    "Right shift");
        buttonDisplayOverrides.Add("leftCtrl",      "Left Ctrl");
        buttonDisplayOverrides.Add("rightCtrl",     "Right Ctrl");
        buttonDisplayOverrides.Add("leftAlt",       "Left alt");
        buttonDisplayOverrides.Add("rightAlt",      "Right alt");

        string value = buttonDisplayOverrides.SingleOrDefault(s => s.Key == name).Value;

        return value == null ? name : value;
    }
}

public class ControlsSettings : UserSettings {
    public bool isUsingKeyboard = false;

    private PlayerCamera playerCamera;

    void OnValidate() {
        ApplySensitivity();
        ApplyBindings();
    }

    void Start() {
        ApplySensitivity();
        ApplyBindings();
    }

    void Update() {
        bool wasUsingKeyboard = isUsingKeyboard;

        if (new Controls().keyboard != null && new Controls().keyboard.anyKey.wasPressedThisFrame) isUsingKeyboard = true;
        if (new Controls().gamepad != null && new Controls().gamepad.wasUpdatedThisFrame) isUsingKeyboard = false;

        if (wasUsingKeyboard != isUsingKeyboard) ApplyBindings();
    }

    public override void PossibilyUpdateFromEvent(SettingsKey key) {
        if (key == SettingsKey.MouseSensitivity && key == SettingsKey.ControllerSensitivity) ApplySensitivity();
        if (key.ToString().Contains("Binding")) ApplyBindings();
    }

    public void ApplySensitivity() {
        if (playerCamera == null) playerCamera = FindFirstObjectByType<PlayerCamera>();
        if (playerCamera == null) return;

        playerCamera.mouseSensitivity = Mathf.Max(Settings.GetSettingInt(SettingsKey.MouseSensitivity, 25) / 10f, 0.01f);
        playerCamera.controllerSensitivity = Mathf.Max(Settings.GetSettingInt(SettingsKey.ControllerSensitivity, 50) * 2f, 0.01f);
    }

    public void RemoveBindings() {
        InputSystem.actions.RemoveAllBindingOverrides();
    }

    public void ApplyBindings() {
        RemoveBindings();

        if (!isUsingKeyboard) return;

        InputAction moveInput = InputSystem.actions.FindAction("Move");

        OverrideMoveComponent(moveInput, "Up",    Settings.GetSettingString(SettingsKey.BindingUp,    "w"));
        OverrideMoveComponent(moveInput, "Down",  Settings.GetSettingString(SettingsKey.BindingDown,  "s"));
        OverrideMoveComponent(moveInput, "Left",  Settings.GetSettingString(SettingsKey.BindingLeft,  "a"));
        OverrideMoveComponent(moveInput, "Right", Settings.GetSettingString(SettingsKey.BindingRight, "d"));

        if (Settings.GetSettingString(SettingsKey.BindingInteract) != "") ApplyAction("Interact", SettingsKey.BindingInteract);
        if (Settings.GetSettingString(SettingsKey.BindingFocus) != "")    ApplyAction("Sprint",   SettingsKey.BindingSprint);
        if (Settings.GetSettingString(SettingsKey.BindingFocus) != "")    ApplyAction("Focus",    SettingsKey.BindingFocus);
        if (Settings.GetSettingString(SettingsKey.BindingDrop) != "")     ApplyAction("Drop",     SettingsKey.BindingDrop);
    }

    private void OverrideMoveComponent(InputAction input, string name, string key) {
        for (int i = 0; i < input.bindings.Count; i++) {
            InputBinding binding = input.bindings[i];

            if (binding.isPartOfComposite && binding.name.ToLower() == name.ToLower()) {
                input.ApplyBindingOverride(i, "<Keyboard>/" + key);
                break;
            }
        }
    }

    private void ApplyAction(string action, SettingsKey settingsKey) {
        string settingsValue = Settings.GetSettingString(settingsKey);

        InputSystem.actions.FindAction(action).ApplyBindingOverride("<Keyboard>/" + Settings.GetSettingString(settingsKey));

        foreach (ButtonControl control in new Controls().mouseControls) {
            if (control.name == settingsValue) {
                InputSystem.actions.FindAction(action).ApplyBindingOverride("<Mouse>/" + Settings.GetSettingString(settingsKey));
            }
        }
    }
}

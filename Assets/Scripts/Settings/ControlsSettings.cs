using UnityEngine;
using UnityEngine.InputSystem;

public class ControlsSettings : UserSettings {
    private PlayerCamera playerCamera;

    void OnValidate() {
        ApplySensitivity();
        ApplyBindings();
    }

    void Start() {
        ApplySensitivity();
        ApplyBindings();
    }

    public override void PossibilyUpdateFromEvent(SettingsKey key) {
        if (key == SettingsKey.MouseSensitivity && key == SettingsKey.ControllerSensitivity) ApplySensitivity();
        if (key == SettingsKey.BindingFocus) ApplyBindings();
    }

    public void ApplySensitivity() {
        if (playerCamera == null) playerCamera = FindFirstObjectByType<PlayerCamera>();

        playerCamera.mouseSensitivity = Mathf.Max(Settings.GetSettingInt(SettingsKey.MouseSensitivity, 25) / 10f, 0.01f);
        playerCamera.controllerSensitivity = Mathf.Max(Settings.GetSettingInt(SettingsKey.ControllerSensitivity, 50) * 2f, 0.01f);
    }

    public void ApplyBindings() {
        InputAction moveInput = InputSystem.actions.FindAction("Move");

        OverrideMoveComponent(moveInput, "Up",    Settings.GetSettingString(SettingsKey.BindingUp,    "w"));
        OverrideMoveComponent(moveInput, "Down",  Settings.GetSettingString(SettingsKey.BindingDown,  "s"));
        OverrideMoveComponent(moveInput, "Left",  Settings.GetSettingString(SettingsKey.BindingLeft,  "a"));
        OverrideMoveComponent(moveInput, "Right", Settings.GetSettingString(SettingsKey.BindingRight, "d"));

        if (Settings.GetSettingString(SettingsKey.BindingInteract) != "") InputSystem.actions.FindAction("Interact").ApplyBindingOverride("<Keyboard>/" + Settings.GetSettingString(SettingsKey.BindingInteract));
        if (Settings.GetSettingString(SettingsKey.BindingFocus) != "")    InputSystem.actions.FindAction("Focus")   .ApplyBindingOverride("<Keyboard>/" + Settings.GetSettingString(SettingsKey.BindingFocus));
        if (Settings.GetSettingString(SettingsKey.BindingDrop) != "")     InputSystem.actions.FindAction("Drop")    .ApplyBindingOverride("<Keyboard>/" + Settings.GetSettingString(SettingsKey.BindingDrop));
    }

    private void OverrideMoveComponent(InputAction input, string name, string key) {
        for (int i = 0; i < input.bindings.Count; i++) {
            InputBinding binding = input.bindings[i];

            if (binding.isPartOfComposite && string.Equals(binding.name, name, System.StringComparison.OrdinalIgnoreCase)) {
                input.ApplyBindingOverride(i, "<Keyboard>/" + key);
                return;
            }
        }
    }
}

using System.Reflection;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LightSettings : UserSettings {
    void Start() {
        SetLightSettings();
    }

    public override void PossibilyUpdateFromEvent(SettingsKey key) {
        if (key != SettingsKey.SimpleLighting) return;

        SetLightSettings();
    }

    public void SetLightSettings() {
        UniversalRenderPipelineAsset urpAsset = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;

        FieldInfo field = typeof(UniversalRenderPipelineAsset).GetField("m_AdditionalLightsRenderingMode", BindingFlags.NonPublic | BindingFlags.Instance);

        field.SetValue(urpAsset, Settings.IsSettingBoolEnabled(SettingsKey.SimpleLighting, false) ? LightRenderingMode.PerVertex : LightRenderingMode.PerPixel);
    }
}

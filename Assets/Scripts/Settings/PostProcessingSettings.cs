using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingSettings : UserSettings {
    void OnValidate() {
        ApplySettings();
    }

    void Start() {
        ApplySettings();
    }

    public override void PossibilyUpdateFromEvent(SettingsKey key) {
        if (key != SettingsKey.Pixelation) return;

        ApplySettings();
    }

    public void ApplySettings() {
        int storedValue = Settings.GetSettingInt(SettingsKey.Pixelation, 0);

        ScriptableRenderer renderer = (GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset).GetRenderer(0);
        PropertyInfo property = typeof(ScriptableRenderer).GetProperty("rendererFeatures", BindingFlags.NonPublic | BindingFlags.Instance);
        List<ScriptableRendererFeature> features = property.GetValue(renderer) as List<ScriptableRendererFeature>;

        foreach (ScriptableRendererFeature feature in features) {
            if (feature.name == "PixelationFx") {
                feature.SetActive(storedValue == 0);
            }
        }
    }
}

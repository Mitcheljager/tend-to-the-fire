using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingSettings : UserSettings {
    private Volume volume;

    void OnValidate() {
        ApplyPixelationSettings();
    }

    void Start() {
        ApplyPixelationSettings();
        SetPostExposureSettings();
    }

    public override void PossibilyUpdateFromEvent(SettingsKey key) {
        if (key == SettingsKey.Pixelation) ApplyPixelationSettings();
        if (key == SettingsKey.PostExposure) SetPostExposureSettings();
    }

    public void ApplyPixelationSettings() {
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

    public void SetPostExposureSettings() {
        if (volume == null) volume = FindFirstObjectByType<Volume>();

        if (volume.profile.TryGet<ColorAdjustments>(out ColorAdjustments colorAdjustments)) {
            colorAdjustments.postExposure.value = Settings.GetSettingFloat(SettingsKey.PostExposure, 0f);
        }
    }
}

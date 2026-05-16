using UnityEngine;
using UnityEngine.VFX;

public class FireEffects : MonoBehaviour {
    public Fire fire;
    [Header("Embers")]
    public VisualEffect embersEffect;
    public AnimationCurve embersEffectRateCurve = new(new Keyframe(0f, 1f), new Keyframe(0.75f, 1f), new Keyframe(1f, 0f));
    public AnimationCurve embersEffectMaxVelocityCurve = new(new Keyframe(0f, 1f), new Keyframe(0.75f, 1f), new Keyframe(1f, 0f));
    public AnimationCurve embersEffectMaxLifetimeCurve = new(new Keyframe(0f, 1f), new Keyframe(0.75f, 1f), new Keyframe(1f, 0f));
    [Header("Smoke")]
    public VisualEffect smokeEffect;
    public AnimationCurve smokeEffectRateCurve = new(new Keyframe(0f, 1f), new Keyframe(0.75f, 1f), new Keyframe(1f, 0f));
    [Header("Fire")]
    public Renderer[] effectRenderers;
    public AnimationCurve effectFuzzinessCurve = new(new Keyframe(0f, 1f), new Keyframe(0.75f, 1f), new Keyframe(1f, 0f));
    public AnimationCurve effectContrastCurve = new(new Keyframe(0f, 1f), new Keyframe(0.75f, 1f), new Keyframe(1f, 0f));
    public AnimationCurve effectIntensityCurve = new(new Keyframe(0f, 1f), new Keyframe(0.75f, 1f), new Keyframe(1f, 0f));

    void Update() {
        SetEffectValues();
    }

    private void SetEffectValues() {
        embersEffect.SetFloat("Rate", embersEffectRateCurve.Evaluate(fire.currentMultiplier));
        embersEffect.SetFloat("Max Y Velocity", embersEffectMaxVelocityCurve.Evaluate(fire.currentMultiplier));
        embersEffect.SetFloat("Max Lifetime", embersEffectMaxLifetimeCurve.Evaluate(fire.currentMultiplier));

        smokeEffect.SetFloat("Rate", smokeEffectRateCurve.Evaluate(fire.currentMultiplier));

        foreach (Renderer effectRenderer in effectRenderers) {
            effectRenderer.material.SetFloat("_Fuzziness", effectFuzzinessCurve.Evaluate(fire.currentMultiplier));
            effectRenderer.material.SetFloat("_Contrast", effectContrastCurve.Evaluate(fire.currentMultiplier));
            effectRenderer.material.SetFloat("_Intensity", effectIntensityCurve.Evaluate(fire.currentMultiplier));
        }
    }
}

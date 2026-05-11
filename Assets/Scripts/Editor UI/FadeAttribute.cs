using UnityEngine;

public class FadeAttribute : PropertyAttribute {
    public float alpha;

    public FadeAttribute(float alpha = 0.65f) {
        this.alpha = Mathf.Clamp01(alpha);
    }
}

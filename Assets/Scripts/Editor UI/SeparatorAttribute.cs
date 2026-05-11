using UnityEngine;

public class SeparatorAttribute : PropertyAttribute {
    public float thickness;
    public float padding;

    public SeparatorAttribute(float thickness = 1f, float padding = 20f) {
        this.thickness = thickness;
        this.padding = padding;
    }
}

using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SeparatorAttribute))]
public class SeparatorDrawer : DecoratorDrawer {
    public override float GetHeight() {
        SeparatorAttribute attr = (SeparatorAttribute)attribute;

        return attr.padding + attr.thickness;
    }

    public override void OnGUI(Rect position) {
        SeparatorAttribute attr = (SeparatorAttribute)attribute;
        float y = position.y + attr.padding;

        Rect line = new(position.x, y, position.width, attr.thickness);

        EditorGUI.DrawRect(line, Color.black);
    }
}

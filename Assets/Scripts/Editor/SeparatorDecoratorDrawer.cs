using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SeparatorAttribute))]
public class SeparatorDrawer : DecoratorDrawer {
    public override float GetHeight() {
        SeparatorAttribute separator = (SeparatorAttribute)attribute;

        return separator.padding + separator.thickness;
    }

    public override void OnGUI(Rect position) {
        SeparatorAttribute separator = (SeparatorAttribute)attribute;
        float y = position.y + separator.padding;

        Rect line = new(position.x, y, position.width, separator.thickness);

        EditorGUI.DrawRect(line, Color.black);
    }
}

using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(FadeAttribute))]
public class FadeDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        FadeAttribute fade = (FadeAttribute)attribute;

        Color oldColor = GUI.color;

        Color color = GUI.color;
        color.a = fade.alpha;
        GUI.color = color;

        EditorGUI.PropertyField(position, property, label, true);

        GUI.color = oldColor;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
}

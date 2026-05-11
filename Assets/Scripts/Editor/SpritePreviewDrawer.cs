using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Sprite))]
public class SpritePreviewDrawer : PropertyDrawer {
    private const float previewSize = 50f;
    private const float padding = 10f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, label);

        Rect objectFieldRect = new(position.x, position.y, position.width - previewSize - padding, EditorGUIUtility.singleLineHeight);

        EditorGUI.PropertyField(objectFieldRect, property, GUIContent.none);

        if (property.objectReferenceValue is Sprite sprite) {
            Rect previewRect = new (objectFieldRect.xMax + padding, position.y, previewSize, previewSize);

            Texture2D texture = AssetPreview.GetAssetPreview(sprite);
            if (texture != null) GUI.DrawTexture(previewRect, texture, ScaleMode.ScaleToFit);
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return Mathf.Max(EditorGUIUtility.singleLineHeight, previewSize);
    }
}

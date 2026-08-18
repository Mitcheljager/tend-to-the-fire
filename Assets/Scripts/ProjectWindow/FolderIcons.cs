using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[InitializeOnLoad]
public class FolderIcons {
    static FolderIcons() {
        EditorApplication.projectWindowItemOnGUI -= RenderFolderIcon;
        EditorApplication.projectWindowItemOnGUI += RenderFolderIcon;
    }

    private static void RenderFolderIcon(string guid, Rect rect) {
        string path = AssetDatabase.GUIDToAssetPath(guid);

        Rect additionalRect;
        Rect imageRect;
        Rect cutoutRect;

        if (rect.height > 20) {
            imageRect = new Rect(rect.x - 1, rect.y - 1, rect.width + 2, rect.width + 2);
            additionalRect = new Rect(rect.x + imageRect.width * 0.325f, rect.y + imageRect.height * 0.375f, imageRect.width * 0.35f, imageRect.height * 0.35f);
            cutoutRect = new Rect(rect.x + imageRect.width * 0.3f, rect.y + imageRect.height * 0.35f, imageRect.width * 0.4f, imageRect.height * 0.4f);
        } else if (rect.x > 20) {
            imageRect = new Rect(rect.x - 1, rect.y - 1, rect.height + 2, rect.height + 2);
            additionalRect = new Rect(rect.x, rect.y, imageRect.width * 0.9f, imageRect.height * 0.9f);
            cutoutRect = additionalRect;
        } else {
            imageRect = new Rect(rect.x + 2, rect.y - 1, rect.height + 2, rect.height + 2);
            additionalRect = new Rect(rect.x * 1.1f, rect.y, imageRect.width * 0.9f, imageRect.height * 0.9f);
            cutoutRect = additionalRect;
        }

        string iconName = "";

        if (path == "Assets/Scripts")          iconName = "cs Script Icon";
        else if (path == "Assets/Materials")   iconName = "d_Material Icon";
        else if (path == "Assets/Shaders")     iconName = "d_Shader Icon";
        else if (path == "Assets/Prefabs")     iconName = "Prefab Icon";
        else if (path == "Assets/Textures")    iconName = "d_Texture Icon";
        else if (path == "Assets/Animations")  iconName = "Animation Icon";
        else if (path == "Assets/Audio")       iconName = "AudioClip Icon";
        else if (path == "Assets/Fonts")       iconName = "d_Font Icon";
        else if (path == "Assets/Terrain")     iconName = "d_TerrainData Icon";
        else if (path == "Assets/Scenes")      iconName = "d_UnityLogo";
        else if (path == "Assets/Images")      iconName = "Image Icon";
        else if (path == "Assets/Effects")     iconName = "d_VisualEffect Icon";
        else if (path == "Assets/Skymap")      iconName = "Skybox Icon";
        else if (path == "Assets/Meshes")      iconName = "d_Mesh Icon";
        else if (path == "Assets/Settings")    iconName = "d_Settings@2x";
        else if (path == "Assets/Packages")    iconName = "PreviewPackageInUse@2x";
        else if (path == "Assets/Gizmos")      iconName = "d_UnityEditor.SceneView@2x";

        if (iconName != "") {
            Texture2D cutoutTexture = new Texture2D(1, 1);
            cutoutTexture.SetPixel(0, 0, Color.gray2);
            cutoutTexture.Apply(false);

            GUI.DrawTexture(cutoutRect, cutoutTexture);
            GUI.DrawTexture(additionalRect, EditorGUIUtility.IconContent(iconName).image as Texture2D);
        }
    }
}
#endif

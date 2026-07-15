using System.IO;
using UnityEditor;
using UnityEngine;

public class ReplacePrefabs : EditorWindow {
    private const string FromToken = "Spruce";
    private const string ToToken = "Pine";

    [MenuItem("Tools/Replace selected prefabs")]
    private static void ReplaceSelected() {
        GameObject[] selection = Selection.gameObjects;

        if (selection.Length == 0) {
            Debug.LogWarning("[ReplacePrefabs] No objects selected.");
            return;
        }

        int successCount = 0;
        int failCount = 0;

        Undo.SetCurrentGroupName("Replace Prefabs");
        int undoGroup = Undo.GetCurrentGroup();

        foreach (GameObject instance in selection) {
            if (TryReplaceInstance(instance, out string error)) {
                successCount++;
            } else {
                failCount++;
                Debug.LogWarning($"[ReplacePrefabs] Skipped '{instance.name}': {error}", instance);
            }
        }

        Undo.CollapseUndoOperations(undoGroup);

        Debug.Log($"[ReplacePrefabs] Done. Replaced: {successCount}, Skipped: {failCount}.");
    }

    private static bool TryReplaceInstance(GameObject instance, out string error) {
        error = null;

        // Find the prefab asset this instance came from.
        GameObject sourcePrefab = PrefabUtility.GetCorrespondingObjectFromSource(instance);
        if (sourcePrefab == null) {
            error = "Not a prefab instance (no source prefab found).";
            return false;
        }

        string sourcePath = AssetDatabase.GetAssetPath(sourcePrefab);
        if (string.IsNullOrEmpty(sourcePath)) {
            error = "Could not resolve asset path for source prefab.";
            return false;
        }

        string sourceAssetName = Path.GetFileNameWithoutExtension(sourcePath);
        if (!sourceAssetName.Contains(FromToken)) {
            error = $"Source prefab name '{sourceAssetName}' does not contain '{FromToken}'.";
            return false;
        }

        string targetAssetName = sourceAssetName.Replace(FromToken, ToToken);
        string folder = Path.GetDirectoryName(sourcePath).Replace('\\', '/');

        GameObject targetPrefab = FindPrefabInFolder(folder, targetAssetName);
        if (targetPrefab == null) {
            error = $"Could not find replacement prefab '{targetAssetName}' in '{folder}'.";
            return false;
        }

        Transform oldTransform = instance.transform;
        Transform parent = oldTransform.parent;
        int siblingIndex = oldTransform.GetSiblingIndex();

        GameObject newInstance = (GameObject)PrefabUtility.InstantiatePrefab(targetPrefab, parent);
        Undo.RegisterCreatedObjectUndo(newInstance, "Replace Prefabs");

        Transform newTransform = newInstance.transform;
        newTransform.localPosition = oldTransform.localPosition;
        newTransform.localRotation = oldTransform.localRotation;
        newTransform.localScale = oldTransform.localScale;
        newTransform.SetSiblingIndex(siblingIndex);

        newInstance.name = instance.name.Replace(FromToken, ToToken);

        Undo.DestroyObjectImmediate(instance);

        return true;
    }

    private static GameObject FindPrefabInFolder(string folder, string prefabName) {
        string[] guids = AssetDatabase.FindAssets($"t:Prefab {prefabName}", new[] { folder });

        foreach (string guid in guids) {
            string path = AssetDatabase.GUIDToAssetPath(guid);

            if (Path.GetFileNameWithoutExtension(path) == prefabName) {
                return AssetDatabase.LoadAssetAtPath<GameObject>(path);
            }
        }

        return null;
    }
}

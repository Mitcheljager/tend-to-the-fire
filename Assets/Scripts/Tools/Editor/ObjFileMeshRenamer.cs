using UnityEditor;
using UnityEngine;

public class ObjFileMeshRenamer : AssetPostprocessor {
    void OnPostprocessModel(GameObject GameObject) {
        if (!assetPath.EndsWith(".obj")) return;

        string meshName = System.IO.Path.GetFileNameWithoutExtension(assetPath);

        foreach (MeshFilter meshFilter in GameObject.GetComponentsInChildren<MeshFilter>()) {
            if (meshFilter.sharedMesh != null) meshFilter.sharedMesh.name = meshName;
        }
    }
}

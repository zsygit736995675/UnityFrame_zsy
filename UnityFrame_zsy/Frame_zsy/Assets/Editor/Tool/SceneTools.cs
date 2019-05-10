using UnityEditor;
using UnityEngine;
using System.Collections;

public class SceneTools : EditorWindow {

    //方法过时了
    //[MenuItem("Tools/Scene/Export Current Scene")]
    //private static void ExportScene()
    //{
    //    string currentScene = EditorApplication.currentScene;
    //    string currentSceneName = currentScene.Substring(currentScene.LastIndexOf('/') + 1, currentScene.LastIndexOf('.') - currentScene.LastIndexOf('/') - 1);
    //    BuildPipeline.BuildStreamedSceneAssetBundle(new string[1] { EditorApplication.currentScene }, "Assets/StreamingAssets/scene/" + currentSceneName + ".ab", EditorUserBuildSettings.activeBuildTarget);
    //}

    [MenuItem("Tools/Scene/RemoveColliders")]
    private static void RemoveColliders()
    {
        foreach (GameObject go in Selection.gameObjects)
        {
            Collider[] colliders = go.GetComponentsInChildren<Collider>();
            foreach (var collider in colliders)
            {
                if (collider is TerrainCollider)
                    continue;
                BakeLightmap();
                DestroyImmediate(collider);
            }
        }
    }

    // 烘焙最大图集尺寸
    [MenuItem("Tools/Scene/Bake Lightmap")]
    private static void BakeLightmap()
    {
        LightmapEditorSettings.maxAtlasHeight = 512;
        LightmapEditorSettings.maxAtlasSize = 512;
        Lightmapping.Clear();
        Lightmapping.Bake();
    }

}

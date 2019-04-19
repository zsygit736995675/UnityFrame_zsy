using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

/// <summary>
/// 打包脚本 by：lsj
/// </summary>
public class Builder : Editor
{
    public static string tablePath = Application.dataPath + "/exp/exp_bytes/";
    public static string modelPath = Application.dataPath + "/Builds/Model4/";

    public static string AssetBundlesOutputPath = Application.dataPath + "/StreamingAssets/";


    [MenuItem("BundleBuilder/Create Congig Asset")]
    public static void CreateSelectedAsset()
    {
        Caching.ClearCache();
        string[] directoryEntries = System.IO.Directory.GetFileSystemEntries(tablePath);

        string outPath = Platform.GetPath(Platform.PathType.Table);
        if (!Directory.Exists(outPath))
        {
            Directory.CreateDirectory(outPath);
        }

        List<Object> assets = new List<Object>();
        for (int i = 0; i < directoryEntries.Length; ++i)
        {
            string p = directoryEntries[i];

            if (!p.EndsWith(".bytes"))
                continue;

            int nStartPos = p.LastIndexOf('/');
            string fileName = p.Substring(nStartPos + 1, p.Length - ".bytes".Length - nStartPos - 1);

            string assetPath = "Assets/exp/exp_bytes/" + fileName + ".bytes";
            Debug.Log(assetPath);
            Object asset = AssetDatabase.LoadMainAssetAtPath(assetPath);
            if (null != asset)
            {
                assets.Add(asset);
                Debug.Log("Add Table Asset: " + fileName);
            }
            else
            {
                Debug.LogError("Create Table Asset Error!!! " + fileName);
            }
        }
        if (assets.Count > 0)
        {
            string targetPath = outPath + "tableCon.assetbundle";
            Debug.Log(targetPath);
            if (BuildPipeline.BuildAssetBundle(null, assets.ToArray(), targetPath,BuildAssetBundleOptions.UncompressedAssetBundle |
                BuildAssetBundleOptions.CompleteAssets,EditorUserBuildSettings.activeBuildTarget))
            {
                Debug.Log("table.assetbundle create assetbundle success!!!");
            }
            else
            {
                Debug.LogError("table.assetbundle create assetbundle failed!!!!");
            }
        }
        
        AssetDatabase.Refresh();
    }

    [MenuItem("BundleBuilder/Create Model Asset")]
    public static void CreateSelectedModel()
    {
        Caching.ClearCache();
        Object[] selectedAssets = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);

        string outPath = Platform.GetPath(Platform.PathType.Model);
        if (!Directory.Exists(outPath))
        {
            Directory.CreateDirectory(outPath);
        }

        foreach (Object asset in selectedAssets)
        {
            string targetPath = outPath + asset.name + ".assetbundle";

            if (BuildPipeline.BuildAssetBundle(asset, null, targetPath, BuildAssetBundleOptions.CollectDependencies |
                BuildAssetBundleOptions.CompleteAssets, EditorUserBuildSettings.activeBuildTarget))
            {
                Debug.Log(asset.name + ": create assetbundle success!!!");
            }
            else
            {
                Debug.Log(asset.name + ": create assetbundle failed!!!");
            }
        }

        AssetDatabase.Refresh();
    }


}


public class Platform
{
    public enum PathType
    {
        Table,
        Model,
    }

    #region 工具类
    public static string GetPath(PathType type)
    {
        string targetPath = "";
        string str = "";
        switch (type)
        {
            case PathType.Table:
                str = "table/";
                break;
            case PathType.Model:
                str = "models/";
                break;
        }
        targetPath = Builder.AssetBundlesOutputPath + GetPlatformFolder(EditorUserBuildSettings.activeBuildTarget) + str;
        return targetPath;
    }

    public static string GetPlatformFolder(BuildTarget target)
    {
        switch (target)
        {
            case BuildTarget.Android:
                return "Android/";
            case BuildTarget.iOS:
                return "IOS/";
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
                return "Windows/";
            case BuildTarget.StandaloneOSXIntel:
            case BuildTarget.StandaloneOSXIntel64:
                return "OSX/";
            default:
                return null;
        }
    }
    #endregion
}

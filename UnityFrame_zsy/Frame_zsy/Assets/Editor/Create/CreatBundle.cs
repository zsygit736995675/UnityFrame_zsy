using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

/// <summary>
/// AssetBundler打包脚本
/// </summary>
public class CreatBundle :MonoBehaviour
{
    private static BuildTarget currentTarget;//当前目标平台

    /// <summary>
    /// bundle All
    /// </summary>
   public  static void ExportAssetBundle(BuildTarget target)
    {
        currentTarget = target;

        //清空缓存
        Caching.ClearCache();

        ClearAssetBundlesName();

        ResetAssetAllMask();

        //打包1.输出路径 2.选项 3.目标平台    
        AssetBundleManifest manifest= BuildPipeline.BuildAssetBundles(PathUtils.AssetBundlesOutPutPath, BuildAssetBundleOptions.DeterministicAssetBundle|BuildAssetBundleOptions.StrictMode, target);

        //编译资源
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// build选中的目标，不使用依赖，强制打包目标所有依赖
    /// </summary>
    public static void ExportSelect(BuildTarget target)
    {
        Caching.ClearCache();
        Object[] selectedAssets = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);

        string outPath = PathUtils.GetAssetOutPath((int)target);
        if (!Directory.Exists(outPath))
        {
            Directory.CreateDirectory(outPath);
        }

        foreach (Object asset in selectedAssets)
        {
            string targetPath = PathUtils.GetAssetOutPath((int)target) + asset.name + ".assetbundle";

            if (BuildPipeline.BuildAssetBundle(asset, null, targetPath, BuildAssetBundleOptions.CollectDependencies |BuildAssetBundleOptions.CompleteAssets, EditorUserBuildSettings.activeBuildTarget))
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

    /// <summary>
    /// 生成表格ab
    /// </summary>
    /// <param name="target"></param>
    public static void ExportConfig(BuildTarget target)
    {
        Caching.ClearCache();
        string[] directoryEntries = System.IO.Directory.GetFileSystemEntries(Application.dataPath+ PathUtils.tablePath);

        string outPath = PathUtils.GetAssetOutPath((int)target);
        if (!Directory.Exists(outPath))
        {
            Directory.CreateDirectory(outPath);
        }

        List<Object> assets = new List<Object>();
        for (int i = 0; i < directoryEntries.Length; ++i)
        {
            string p = directoryEntries[i];

            if (!p.EndsWith(".bytes"))
            {
                continue;
            }

            int nStartPos = p.LastIndexOf('/');
            string fileName = p.Substring(nStartPos + 1, p.Length - ".bytes".Length - nStartPos - 1);
            string assetPath = "Assets/exp/exp_bytes/" + fileName + ".bytes";

            Object asset = AssetDatabase.LoadMainAssetAtPath(assetPath);
            if (null != asset)
            {
                assets.Add(asset);
            }
            else
            {
                Debug.LogError("Create Table Asset Error!!! " + fileName);
            }
        }
        if (assets.Count > 0)
        {
            string targetPath = outPath + "tableCon.assetbundle";
            if (BuildPipeline.BuildAssetBundle(null, assets.ToArray(), targetPath, BuildAssetBundleOptions.UncompressedAssetBundle | BuildAssetBundleOptions.CompleteAssets | BuildAssetBundleOptions.DeterministicAssetBundle, EditorUserBuildSettings.activeBuildTarget))
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

    /// <summary>
    ///刷新配置文件
    /// </summary>
    public static void ResetHashFile()
    {

    }

    /// <summary>
    /// 清除之前设置过的AssetBundleName，避免产生不必要的资源也打包
    /// </summary>
    public static void ClearAssetBundlesName()
    {
        int length = AssetDatabase.GetAllAssetBundleNames().Length;

        string[] oldAssetBundleNames = new string[length];
        for (int i = 0; i < length; i++)
        {
            oldAssetBundleNames[i] = AssetDatabase.GetAllAssetBundleNames()[i];
        }

        for (int j = 0; j < oldAssetBundleNames.Length; j++)
        {
            AssetDatabase.RemoveAssetBundleName(oldAssetBundleNames[j], true);
        }

        Debug.Log("Cleard BundleName Count: " + length);
    }

    /// <summary>
    /// 刷新所有标识
    /// </summary>
    public static void ResetAssetAllMask()
    {
        string[] names =Directory.GetDirectories (Application.dataPath + PathUtils.bundleFilePathRoot);
       
        foreach (string item in names)
        {
            ExportAssetName(item);
        }

        string[] configPath = Directory.GetDirectories(Application.dataPath + "/exp/");

        foreach (string item in configPath)
        {
            ExportAssetName(item);
        }
    }

    /// <summary>
    /// 生成标识
    /// </summary>
    private static void ExportAssetName(string resPath)
    {
        List<string> pathFiles = new List<string>();

        Recursive(resPath, pathFiles);
        if (pathFiles.Count > 0)
        {
            string resName = PathUtils.GetPlatformName((int)currentTarget);
            SetAssetBundlesName(pathFiles, resName);
        }
    }

    /// <summary>
    /// 旧的设置标识
    /// </summary>
    private void oldMethod()
    {
        //ExportAssetCommonTexture();
        //ExportAssetCommonScene();
        //ExportAssetCommonFont();
        //ExportAssetCommonUI();
        //ExportAssetCommonImage();
        //ExportAssetCommonEffect();
        //ExportAssetCommonAnimation();
        //ExportAssetCommonRole();
        //ExportAssetCommonAudio();
        //ExportAssetCommonConfig();
        //ExportAssetCommonOther();
    }

    ////贴图
    //private static void ExportAssetCommonTexture()
    //{
    //    List<string> tabPaths = ToolsConst.GetCommonSceneTexturePath();
    //    List<string> paths = new List<string>();
    //    List<string> pathFiles = new List<string>();
    //    List<string> includeNames = new List<string>();
    //    includeNames.Add(".renderTexture");
    //    foreach (var v in tabPaths)
    //    {
    //        Recursive(v, paths, pathFiles, includeNames);
    //    }
    //    SetAssetBundleName(pathFiles, "texture");
    //}

    ////场景
    //private static void ExportAssetCommonScene()
    //{
    //    List<string> tabPaths = ToolsConst.GetCommonSceneFilePath();
    //    List<string> paths = new List<string>();
    //    List<string> pathFiles = new List<string>();
    //    List<string> includeNames = new List<string>();
    //    includeNames.Add(".unity");
    //    foreach (var v in tabPaths)
    //    {
    //        Recursive(v, paths, pathFiles, includeNames);
    //    }
    //    SetAssetBundleName(pathFiles, "scene");
    //}

    ////通用UI
    //static void ExportAssetCommonUI()
    //{
    //    List<string> tabPaths = ToolsConst.GetCommonUIFilePath();
    //    List<string> paths = new List<string>();
    //    List<string> pathFiles = new List<string>();
    //    List<string> includeNames = new List<string>();
    //    includeNames.Add(".prefab");
    //    foreach (var v in tabPaths)
    //    {
    //        Recursive(v, paths, pathFiles, includeNames);
    //    }
    //    //给文件设置AssetBundleName
    //    ActivePrefab(pathFiles, "ui");
    //}

    ////Role
    //static void ExportAssetCommonRole()
    //{
    //    List<string> tabPaths = ToolsConst.GetCommonUIRolePath();
    //    List<string> paths = new List<string>();
    //    List<string> pathFiles = new List<string>();
    //    List<string> includeNames = new List<string>();
    //    includeNames.Add(".prefab");
    //    foreach (var v in tabPaths)
    //    {
    //        Recursive(v, paths, pathFiles, includeNames);
    //    }
    //    //给文件设置AssetBundleName
    //    SetAssetBundleName(pathFiles, "role");
    //}

    ////图片
    //static void ExportAssetCommonImage()
    //{
    //    List<string> tabPaths = ToolsConst.GetCommonImageFilePath();
    //    List<string> paths = new List<string>();
    //    List<string> pathFiles = new List<string>();
    //    List<string> includeNames = new List<string>();
    //    includeNames.Add(".png");
    //    includeNames.Add(".jpg");
    //    includeNames.Add(".bmp");
    //    includeNames.Add(".tga");
    //    foreach (var v in tabPaths)
    //    {
    //        Recursive(v, paths, pathFiles, includeNames);
    //    }
    //    //给文件设置AssetBundleName
    //    SetAssetBundlesName(pathFiles, "icon");
    //}

    ////表格
    //static void ExportAssetCommonConfig() {
    //    List<string> tabPaths = ToolsConst.GetCommonConfigFilePath();
    //    List<string> paths = new List<string>();
    //    List<string> pathFiles = new List<string>();
    //    List<string> includeNames = new List<string>();
    //    includeNames.Add(".bytes");
    //    foreach (var v in tabPaths)
    //    {
    //        Recursive(v, paths, pathFiles, includeNames);
    //    }
    //    SetAssetBundlesName(pathFiles,"config");
    //}


    ////字体
    //static void ExportAssetCommonFont()
    //{
    //    List<string> tabPaths = ToolsConst.GetCommonFontFilePath();
    //    List<string> paths = new List<string>();
    //    List<string> pathFiles = new List<string>();
    //    List<string> includeNames = new List<string>();
    //    includeNames.Add(".ttf");
    //    includeNames.Add(".TTF");
    //    foreach (var v in tabPaths)
    //    {
    //        Recursive(v, paths, pathFiles, includeNames);
    //    }
    //    //给文件设置AssetBundleName
    //    SetAssetBundlesName(pathFiles, "fonts");
    //}

    ////声音
    //static void ExportAssetCommonAudio()
    //{
    //    List<string> tabPaths = ToolsConst.GetCommonAudioFilePath();
    //    List<string> paths = new List<string>();
    //    List<string> pathFiles = new List<string>();
    //    List<string> includeNames = new List<string>();

    //    includeNames.Add(".mp3");
    //    includeNames.Add(".wav");
    //    includeNames.Add(".ogg");
    //    includeNames.Add(".aiff");
    //    foreach (var v in tabPaths)
    //    {
    //        Recursive(v, paths, pathFiles, includeNames);
    //    }
    //    //给文件设置AssetBundleName
    //    SetAssetBundleName(pathFiles, "audio");
    //}

    ////通用特效
    //static void ExportAssetCommonEffect()
    //{
    //    List<string> tabPaths = ToolsConst.GetCommonEffectFilePath();
    //    List<string> paths = new List<string>();
    //    List<string> pathFiles = new List<string>();
    //    List<string> includeNames = new List<string>();

    //    includeNames.Add(".prefab");
    //    foreach (var v in tabPaths)
    //    {
    //        Recursive(v, paths, pathFiles, includeNames);
    //    }
    //    //给文件设置AssetBundleName
    //    SetAssetBundleName(pathFiles, "effect");
    //}

    ////通用动画
    //static void ExportAssetCommonAnimation()
    //{
    //    List<string> tabPaths = ToolsConst.GetCommonAnimationFilePath();
    //    List<string> paths = new List<string>();
    //    List<string> pathFiles = new List<string>();
    //    List<string> includeNames = new List<string>();

    //    includeNames.Add(".anim");
    //    includeNames.Add(".controller");
    //    foreach (var v in tabPaths)
    //    {
    //        Recursive(v, paths, pathFiles, includeNames);
    //    }
    //    //给文件设置AssetBundleName
    //    SetAssetBundlesName(pathFiles, "anim");
    //}


    ////other
    //static void ExportAssetCommonOther()
    //{
    //    List<string> tabPaths = ToolsConst.GetCommonOtherFilePath();
    //    List<string> paths = new List<string>();
    //    List<string> pathFiles = new List<string>();
    //    List<string> includeNames = new List<string>();

    //    includeNames.Add(".prefab");
    //    includeNames.Add(".png");
    //    foreach (var v in tabPaths)
    //    {
    //        Recursive(v, paths, pathFiles, includeNames);
    //    }
    //    //给文件设置AssetBundleName
    //    SetAssetBundleName(pathFiles, "other");
    //}

    //设置assetBundleName

    /// <summary>
    /// 对单个物体设置标识，标识就是物体名称
    /// </summary>
    /// <param name="pathFiles"></param>
    /// <param name="name"></param>
    static void SetAssetBundleName(List<string> pathFiles, string name) {
        for (int i = 0, max = pathFiles.Count; i < max; i++)
        {
            string strBundleName = FileUtils.GetFileName(pathFiles[i]);
            string newfile = pathFiles[i].Replace(Application.dataPath, "Assets");
            //在代码中给资源设置AssetBundleName
            AssetImporter assetImporter = AssetImporter.GetAtPath(newfile);
            assetImporter.assetBundleName = name + "/" + strBundleName + ".ab";
            //assetImporter.assetBundleVariant = "ab";
            EditorUtility.DisplayProgressBar(strBundleName, "", (float)i / (float)max);
        }
    }

    /// <summary>
    /// 对同个文件夹下的文件设置标识，标识为文件夹名称
    /// </summary>
    /// <param name="pathFiles"></param>
    /// <param name="name"></param>
    static void SetAssetBundlesName(List<string> pathFiles, string name)
    {
        for (int i = 0, max = pathFiles.Count; i < max; i++)
        {
            string strBundleName = FileUtils.GetParentFileName(pathFiles[i], 2);

            string newfile = pathFiles[i].Replace(Application.dataPath, "Assets");

            //在代码中给资源设置AssetBundleName
            AssetImporter assetImporter = AssetImporter.GetAtPath(newfile);
            assetImporter.assetBundleName = name + "/" + strBundleName.ToLower() ;//bundle名
            assetImporter.assetBundleVariant = "unity3d";//后缀名
        
            EditorUtility.DisplayProgressBar(strBundleName, "", (float)i / (float)max);
        }
    }

    /// <summary>
    /// 递归获取需要设置标识的文件
    /// </summary>
    /// <param name="recPath">根路径</param>
    /// <param name="paths">没用</param>
    /// <param name="pathFiles">返回文件路径的list</param>
    /// <param name="includeNames">指定后缀名称列表</param>
    static void Recursive(string recPath, List<string> paths, List<string> pathFiles, List<string> includeNames)
    {
        string[] names = Directory.GetFiles(recPath);
        string[] dirs = Directory.GetDirectories(recPath);
        foreach (string filename in names)
        {
            string ext = Path.GetExtension(filename);
            if (!includeNames.Contains(ext))
            {
                continue;
            }
            pathFiles.Add(filename.Replace('\\', '/'));
        }
        foreach (string dir in dirs)
        {
            paths.Add(dir.Replace('\\', '/'));
            Recursive(dir, paths, pathFiles, includeNames);
        }
    }

    /// <summary>
    /// 递归获取需打包文件路径列表
    /// </summary>
    /// <param name="recPath">根路径</param>
    /// <param name="pathFiles">用于返回</param>
    static void Recursive(string recPath, List<string> pathFiles)
    {
        //所有文件
        string[] names = Directory.GetFiles(recPath);
        //所有路径
        string[] dirs = Directory.GetDirectories(recPath);
        foreach (string filename in names)
        {
            string ext = Path.GetExtension(filename);
            if (ext.Contains(".meta") || ext.Contains(".cs") || ext.Contains(".js"))
            {
                continue;
            }
            pathFiles.Add(filename.Replace('\\', '/'));
        }
        foreach (string dir in dirs)
        {
            Recursive(dir, pathFiles);
        }
    }
   
   

    static void ActivePrefab(List<string> pathFiles, string name)
    {
        if (!Directory.Exists(Application.dataPath + "/empUIPrefab/"))
        Directory.CreateDirectory(Application.dataPath + "/tempUIPrefab/");
            for (int i = 0, max = pathFiles.Count; i < max; i++)
           {
            string strBundleName = FileUtils.GetFileName(pathFiles[i]);
            string newfile = pathFiles[i].Replace(Application.dataPath, "Assets");
            //在代码中给资源设置AssetBundleName

            GameObject rootGo = AssetDatabase.LoadAssetAtPath(newfile, typeof(GameObject)) as GameObject;
            GameObject root = Instantiate(rootGo) as GameObject;
            root.name = rootGo.name;
            Activate(root.transform);
            root.SetActive(true);

            string tmpPrefabName = ToolsConst.TMPPREFABPATH + strBundleName + ".prefab";

            PrefabUtility.CreatePrefab(tmpPrefabName, root);
            //AssetDatabase.AddObjectToAsset(, tmpPrefabName);
            AssetDatabase.Refresh();
            AssetImporter assetImporter = AssetImporter.GetAtPath(tmpPrefabName);
            assetImporter.assetBundleName = name + "/" + strBundleName + ".ab";
            
            DestroyImmediate(root);
            EditorUtility.DisplayProgressBar(strBundleName, "", (float)i / (float)max);
        }
    }

    private static void Activate(Transform t)
    {
        t.gameObject.SetActive(true);
        for (int i = 0, imax = t.childCount; i < imax; ++i)
        {
            Transform child = t.GetChild(i);
            Activate(child);
        }
    }


}
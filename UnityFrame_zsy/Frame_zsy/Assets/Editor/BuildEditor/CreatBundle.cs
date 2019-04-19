using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
public class CreatBundleWindow : EditorWindow
{
    [MenuItem("Tools/ExportAssetBundle")]
    static void ExportAssetBundle()
    {
        //清空缓存
        Caching.ClearCache();
        //清除之前设置的名字
        ClearAssetBundlesName();

        //设置ab包名
        ExportAssetCommonTexture();

        ExportAssetCommonScene();

        ExportAssetCommonFont();

        ExportAssetCommonUI();

        ExportAssetCommonImage();

        ExportAssetCommonEffect();

      //  ExportAssetCommonAnimation();

        ExportAssetCommonRole();

        ExportAssetCommonAudio();

        ExportAssetCommonConfig();
        // ExportAssetCommonOther();

        //打包1.输出路径 2.选项 3.目标平台     EditorUserBuildSettings.activeBuildTarget
       // BuildPipeline.BuildAssetBundles(ToolsConst.AssetBundlesOutPutPath, BuildAssetBundleOptions.None, BuildTarget.Android);
       BuildPipeline.BuildAssetBundles(ToolsConst.AssetBundlesOutPutPath, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);

        AssetDatabase.Refresh();

        if (Directory.Exists(Application.dataPath + "/tempUIPrefab/"))
            System.IO.Directory.Delete(Application.dataPath + "/tempUIPrefab", true);

    }



    //取消进度条
    [MenuItem("Tools/CleanProgressBar")]
    private static void CleanProgressBar() {
        EditorUtility.ClearProgressBar();
    }

    //贴图
    private static void ExportAssetCommonTexture()
    {
        List<string> tabPaths = ToolsConst.GetCommonSceneTexturePath();
        List<string> paths = new List<string>();
        List<string> pathFiles = new List<string>();
        List<string> includeNames = new List<string>();
        includeNames.Add(".renderTexture");
        foreach (var v in tabPaths)
        {
            Recursive(v, paths, pathFiles, includeNames);
        }
        SetAssetBundleName(pathFiles, "texture");
    }

    //场景
    private static void ExportAssetCommonScene()
    {
        List<string> tabPaths = ToolsConst.GetCommonSceneFilePath();
        List<string> paths = new List<string>();
        List<string> pathFiles = new List<string>();
        List<string> includeNames = new List<string>();
        includeNames.Add(".unity");
        foreach (var v in tabPaths)
        {
            Recursive(v, paths, pathFiles, includeNames);
        }
        SetAssetBundleName(pathFiles, "scene");
    }

    //通用UI
    static void ExportAssetCommonUI()
    {
        List<string> tabPaths = ToolsConst.GetCommonUIFilePath();
        List<string> paths = new List<string>();
        List<string> pathFiles = new List<string>();
        List<string> includeNames = new List<string>();
        includeNames.Add(".prefab");
        foreach (var v in tabPaths)
        {
            Recursive(v, paths, pathFiles, includeNames);
        }
        //给文件设置AssetBundleName
        ActivePrefab(pathFiles, "ui");
    }

    //Role
    static void ExportAssetCommonRole()
    {
        List<string> tabPaths = ToolsConst.GetCommonUIRolePath();
        List<string> paths = new List<string>();
        List<string> pathFiles = new List<string>();
        List<string> includeNames = new List<string>();
        includeNames.Add(".prefab");
        foreach (var v in tabPaths)
        {
            Recursive(v, paths, pathFiles, includeNames);
        }
        //给文件设置AssetBundleName
        SetAssetBundleName(pathFiles, "role");
    }

    //图片
    static void ExportAssetCommonImage()
    {
        List<string> tabPaths = ToolsConst.GetCommonImageFilePath();
        List<string> paths = new List<string>();
        List<string> pathFiles = new List<string>();
        List<string> includeNames = new List<string>();
        includeNames.Add(".png");
        includeNames.Add(".jpg");
        includeNames.Add(".bmp");
        includeNames.Add(".tga");
        foreach (var v in tabPaths)
        {
            Recursive(v, paths, pathFiles, includeNames);
        }
        //给文件设置AssetBundleName
        SetAssetBundlesName(pathFiles, "icon");
    }

    //表格
    static void ExportAssetCommonConfig() {
        List<string> tabPaths = ToolsConst.GetCommonConfigFilePath();
        List<string> paths = new List<string>();
        List<string> pathFiles = new List<string>();
        List<string> includeNames = new List<string>();
        includeNames.Add(".bytes");
        foreach (var v in tabPaths)
        {
            Recursive(v, paths, pathFiles, includeNames);
        }
        SetAssetBundlesName(pathFiles,"config");
    }


    //字体
    static void ExportAssetCommonFont()
    {
        List<string> tabPaths = ToolsConst.GetCommonFontFilePath();
        List<string> paths = new List<string>();
        List<string> pathFiles = new List<string>();
        List<string> includeNames = new List<string>();
        includeNames.Add(".ttf");
        includeNames.Add(".TTF");
        foreach (var v in tabPaths)
        {
            Recursive(v, paths, pathFiles, includeNames);
        }
        //给文件设置AssetBundleName
        SetAssetBundlesName(pathFiles, "fonts");
    }

    //声音
    static void ExportAssetCommonAudio()
    {
        List<string> tabPaths = ToolsConst.GetCommonAudioFilePath();
        List<string> paths = new List<string>();
        List<string> pathFiles = new List<string>();
        List<string> includeNames = new List<string>();

        includeNames.Add(".mp3");
        includeNames.Add(".wav");
        includeNames.Add(".ogg");
        includeNames.Add(".aiff");
        foreach (var v in tabPaths)
        {
            Recursive(v, paths, pathFiles, includeNames);
        }
        //给文件设置AssetBundleName
        SetAssetBundleName(pathFiles, "audio");
    }

    //通用特效
    static void ExportAssetCommonEffect()
    {
        List<string> tabPaths = ToolsConst.GetCommonEffectFilePath();
        List<string> paths = new List<string>();
        List<string> pathFiles = new List<string>();
        List<string> includeNames = new List<string>();

        includeNames.Add(".prefab");
        foreach (var v in tabPaths)
        {
            Recursive(v, paths, pathFiles, includeNames);
        }
        //给文件设置AssetBundleName
        SetAssetBundleName(pathFiles, "effect");
    }

    //通用动画
    static void ExportAssetCommonAnimation()
    {
        List<string> tabPaths = ToolsConst.GetCommonAnimationFilePath();
        List<string> paths = new List<string>();
        List<string> pathFiles = new List<string>();
        List<string> includeNames = new List<string>();

        includeNames.Add(".anim");
        includeNames.Add(".controller");
        foreach (var v in tabPaths)
        {
            Recursive(v, paths, pathFiles, includeNames);
        }
        //给文件设置AssetBundleName
        SetAssetBundlesName(pathFiles, "anim");
    }

  
    //other
    static void ExportAssetCommonOther()
    {
        List<string> tabPaths = ToolsConst.GetCommonOtherFilePath();
        List<string> paths = new List<string>();
        List<string> pathFiles = new List<string>();
        List<string> includeNames = new List<string>();

        includeNames.Add(".prefab");
        includeNames.Add(".png");
        foreach (var v in tabPaths)
        {
            Recursive(v, paths, pathFiles, includeNames);
        }
        //给文件设置AssetBundleName
        SetAssetBundleName(pathFiles, "other");
    }

    //设置assetBundleName
    static void SetAssetBundleName(List<string> pathFiles, string name) {
        for (int i = 0, max = pathFiles.Count; i < max; i++)
        {
            string strBundleName = EditorTools.GetFileName(pathFiles[i]);
            string newfile = pathFiles[i].Replace(Application.dataPath, "Assets");
            //在代码中给资源设置AssetBundleName
            AssetImporter assetImporter = AssetImporter.GetAtPath(newfile);
            assetImporter.assetBundleName = name + "/" + strBundleName + ".ab";
            //assetImporter.assetBundleVariant = "ab";
            EditorUtility.DisplayProgressBar(strBundleName, "", (float)i / (float)max);
        }
    }

    //设置assetBundleName,图片的，取父文件夹名字
    static void SetAssetBundlesName(List<string> pathFiles, string name)
    {
        for (int i = 0, max = pathFiles.Count; i < max; i++)
        {
            string strBundleName = EditorTools.GetParentFileName(pathFiles[i], 2);
            string newfile = pathFiles[i].Replace(Application.dataPath, "Assets");
            //在代码中给资源设置AssetBundleName
            AssetImporter assetImporter = AssetImporter.GetAtPath(newfile);
            assetImporter.assetBundleName = name + "/" + strBundleName.ToLower() + ".ab";
            //assetImporter.assetBundleVariant = "ab";
            EditorUtility.DisplayProgressBar(strBundleName, "", (float)i / (float)max);
        }
    }

    /// <summary>
    /// 遍历目录及其子目录
    /// </summary>
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

    static void Recursive(string recPath, List<string> paths, List<string> pathFiles)
    {
        string[] names = Directory.GetFiles(recPath);
        string[] dirs = Directory.GetDirectories(recPath);
        foreach (string filename in names)
        {
            //string ext = Path.GetExtension(filename);
            pathFiles.Add(filename.Replace('\\', '/'));
        }
        foreach (string dir in dirs)
        {
            paths.Add(dir.Replace('\\', '/'));
            Recursive(dir, paths, pathFiles);
        }
    }

    /// <summary>
    /// 清除之前设置过的AssetBundleName，避免产生不必要的资源也打包
    /// </summary>
    static void ClearAssetBundlesName()
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
   
    //去掉所有UIPrefab上的脚本
    private static void CleanScripts(GameObject go)
    {
        if (go == null)
            return;

        foreach (Transform child in go.transform)
        {
            CleanScripts(child.gameObject);
        }
    }

    private static void CleanScript<T>(GameObject go) where T : Component
    {
        T t = go.GetComponent<T>();
        while (t != null)
        {
            UnityEngine.MonoBehaviour.DestroyImmediate(t);
            t = go.GetComponent<T>();
        }
    }

    static void ActivePrefab(List<string> pathFiles, string name)
    {
        if (!Directory.Exists(Application.dataPath + "/empUIPrefab/"))
        Directory.CreateDirectory(Application.dataPath + "/tempUIPrefab/");
            for (int i = 0, max = pathFiles.Count; i < max; i++)
           {
            string strBundleName = EditorTools.GetFileName(pathFiles[i]);
            string newfile = pathFiles[i].Replace(Application.dataPath, "Assets");
            //在代码中给资源设置AssetBundleName

            GameObject rootGo = AssetDatabase.LoadAssetAtPath(newfile, typeof(GameObject)) as GameObject;
            GameObject root = Instantiate(rootGo) as GameObject;
            root.name = rootGo.name;
            Activate(root.transform);
            root.SetActive(true);

            string tmpPrefabName = ToolsConst.TMPPREFABPATH + strBundleName + ".prefab";

            PrefabUtility.CreatePrefab(tmpPrefabName, root);
           // AssetDatabase.AddObjectToAsset(, tmpPrefabName);
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
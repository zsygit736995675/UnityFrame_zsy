using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// 编辑器扩展类，放所有按钮
/// </summary>
public class EditorMenu : Editor {

    //-----------------------------------------------------AssetBundler包-------------------------------------------------

    /// <summary>
    /// 生成android AB包
    /// </summary>
    [MenuItem("Tools/ExportAssetBundle/ALL/ExAndroid", false, 1)]
    static void BundleAndroid()
    {
        CreatBundle.ExportAssetBundle(BuildTarget.Android);
    }

    /// <summary>
    /// 生成ios AB包
    /// </summary>
    [MenuItem("Tools/ExportAssetBundle/ALL/ExIOS", false, 1)]
    static void BundleIos()
    {
        CreatBundle.ExportAssetBundle(BuildTarget.iOS);
    }

    /// <summary>
    /// 生成windows AB包
    /// </summary>
    [MenuItem("Tools/ExportAssetBundle/ALL/ExWindows", false, 1)]
    static void BundleWindows()
    {
        CreatBundle.ExportAssetBundle(EditorUserBuildSettings.activeBuildTarget);
    }


    /// <summary>
    /// 生成android config
    /// </summary>
    [MenuItem("Tools/ExportAssetBundle/Config/ExAndroid", false, 1)]
    static void BundleAndroidConfig()
    {
        CreatBundle.ExportConfig(BuildTarget.Android);
    }

    /// <summary>
    /// 生成ios config
    /// </summary>
    [MenuItem("Tools/ExportAssetBundle/Config/ExIOS", false, 1)]
    static void BundleIosConfig()
    {
        CreatBundle.ExportConfig(BuildTarget.iOS);
    }

    /// <summary>
    /// 生成windows config
    /// </summary>
    [MenuItem("Tools/ExportAssetBundle/Config/ExWindows", false, 1)]
    static void BundleWindowsConfig()
    {
        CreatBundle.ExportConfig(EditorUserBuildSettings.activeBuildTarget);
    }


    /// <summary>
    /// 生成android 选中
    /// </summary>
    [MenuItem("Tools/ExportAssetBundle/Select/ExAndroid", false, 1)]
    static void BundleAndroidSelect()
    {
        CreatBundle.ExportSelect(BuildTarget.Android);
    }

    /// <summary>
    /// 生成ios config
    /// </summary>
    [MenuItem("Tools/ExportAssetBundle/Select/ExIOS", false, 1)]
    static void BundleIosSelect()
    {
        CreatBundle.ExportSelect(BuildTarget.iOS);
    }

    /// <summary>
    /// 生成windows config
    /// </summary>
    [MenuItem("Tools/ExportAssetBundle/Select/ExWindows", false, 1)]
    static void BundleWindowsSelect()
    {
        CreatBundle.ExportSelect(EditorUserBuildSettings.activeBuildTarget);
    }


    /// <summary>
    /// 清空缓存
    /// </summary>
    [MenuItem("Tools/ExportAssetBundle/ClearCache", false, 1)]
    static void ClearCache()
    {
        Caching.ClearCache();
    }

    /// <summary>
    /// 清空标识
    /// </summary>
    [MenuItem("Tools/ExportAssetBundle/ClearMark", false, 1)]
    static void ClearMark()
    {
        CreatBundle.ClearAssetBundlesName();
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 重设标识
    /// </summary>
    [MenuItem("Tools/ExportAssetBundle/ResetMark", false, 1)]
    static void ResetMark()
    {
        CreatBundle.ResetAssetAllMask();
    }

    /// <summary>
    /// 刷新配置文件
    /// </summary>
    [MenuItem("Tools/ExportAssetBundle/ResetHashFile/Android", false, 1)]
    static void ResetHashFileAndroid()
    {
        CreatBundle.ResetHashFile(BuildTarget.Android);
    }

    /// <summary>
    /// 刷新配置文件
    /// </summary>
    [MenuItem("Tools/ExportAssetBundle/ResetHashFile/Ios", false, 1)]
    static void ResetHashFileIos()
    {
        CreatBundle.ResetHashFile(BuildTarget.iOS);
    }

    /// <summary>
    /// 刷新配置文件
    /// </summary>
    [MenuItem("Tools/ExportAssetBundle/ResetHashFile/Win", false, 1)]
    static void ResetHashFileWin()
    {
        CreatBundle.ResetHashFile(BuildTarget.StandaloneWindows64);
    }

    /// <summary>
    /// 取消进度条
    /// </summary>
    [MenuItem("Tools/CleanProgressBar", false, 1)]
    private static void CleanProgressBar()
    {
        EditorUtility.ClearProgressBar();
    }

    //---------------------------------------------------------------Create---------------------------------------------------------------------

    /// <summary>
    /// 编辑器入口
    /// </summary>
    [MenuItem("Tools/Create/CreateScript", false, 12)]
    static void CreateScript()
    {
        CreateUiScripts.CreateScript();
    }

    /// <summary>
    /// 生成bmFont
    /// </summary>
    [MenuItem("Tools/Create/CreateBMFont", false, 12)]
    static void CreateFont()
    {
        CreateFontEditor.CreateFont();
    }


    //---------------------------------------------------------------Tool---------------------------------------------------------------------

    /// <summary>
    /// 颜色 转换
    /// </summary>
    [MenuItem("Tools/ColorAndCode", false, 23)]
    static void ColorAndCode()
    {
        ColorCode.init();
    }

    /// <summary>
    /// 分辨率修改
    /// </summary>
    [MenuItem("Tools/Resolution Changer")]
    static void ResoChanger()
    {
        ResolutionChanger.Init();
    }
}

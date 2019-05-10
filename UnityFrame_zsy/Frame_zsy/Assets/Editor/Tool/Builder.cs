using UnityEditor;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// 打包应用
/// </summary>
public static class Builder
{
    public const string PackageName = "zsy_frame";
    public const string AndroidPath = "ExportProject/Android/";
    public const string IosPath = "ExportProject/Ios/";

    public static void BuildAndroid()
    {
        string qudao = GetArgValue("qudao").ToLower();
        string bundleId = GetArgValue("bundleId");
        string iconname = GetArgValue("icon");

        Texture2D icon = AssetDatabase.LoadAssetAtPath("Assets/logo/" + iconname + ".png", typeof(Texture2D)) as Texture2D;
        int[] sizes = PlayerSettings.GetIconSizesForTargetGroup(BuildTargetGroup.Android);
        Texture2D[] icons = new Texture2D[sizes.Length];
        for (int i = 0; i < icons.Length; ++i)
        {
            icons[i] = icon;
        }

        PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Android, icons);
        PlayerSettings.applicationIdentifier = bundleId;
        
    
        StreamReader st = new StreamReader("Assets/StreamingAssets/res/version.txt");
        string version = st.ReadLine().Trim();
        PlayerSettings.bundleVersion = version;
        st.Close();
        PlayerSettings.Android.bundleVersionCode = System.Int32.Parse(version.Replace(".", ""));

        string keystoreName = "android";
        string keystorePwd = "wqgame";
        string keyaliasName = "android.keystore";

        PlayerSettings.productName = "刀塔3";
        PlayerSettings.Android.keystoreName = "../../../android/keystore/" + keystoreName + ".keystore";
        PlayerSettings.Android.keystorePass = keystorePwd;
        PlayerSettings.Android.keyaliasName = keyaliasName;
        PlayerSettings.Android.keyaliasPass = keystorePwd;

        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, "PLATFORM_ANDROID,PLATFORM_" + qudao.ToUpper());
        BuildPipeline.BuildPlayer(GetLevels(), PackageName + ".apk", BuildTarget.Android, BuildOptions.None);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, "");
    }

    public static void BuildIOS()
    {
        string qudao = GetArgValue("qudao");
        string bundleId = GetArgValue("bundleId");
        string iconname = GetArgValue("icon");
        string projectPath = GetArgValue("outpath");

        Texture2D icon = AssetDatabase.LoadAssetAtPath("Assets/logo/" + iconname + ".png", typeof(Texture2D)) as Texture2D;
        int[] sizes = PlayerSettings.GetIconSizesForTargetGroup(BuildTargetGroup.iOS);
        Texture2D[] icons = new Texture2D[sizes.Length];
        for (int i = 0; i < icons.Length; ++i)
        {
            icons[i] = icon;
     
        }

        PlayerSettings.productName = "刀塔3";
        PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.iOS, icons);
        PlayerSettings.applicationIdentifier = bundleId;
        StreamReader st = new StreamReader("Assets/StreamingAssets/res/version.txt");
        string bundleVersion = st.ReadLine().Trim();      
        if (qudao == "pp")
        {
            int index = bundleVersion.LastIndexOf('.');
            bundleVersion = bundleVersion.Substring(0, index) + bundleVersion.Substring(index + 1);
            PlayerSettings.bundleVersion = bundleVersion;
        }
        else
        {
            PlayerSettings.bundleVersion = bundleVersion;
        }       
        st.Close();
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, "PLATFORM_IOS,PLATFORM_" + qudao.ToUpper());
        BuildPipeline.BuildPlayer(GetLevels(), projectPath, BuildTarget.iOS, BuildOptions.AcceptExternalModificationsToPlayer | BuildOptions.SymlinkLibraries);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, "");
    }

    /// <summary>
    /// 切换为测试状态
    /// </summary>
    [MenuItem("Titan/Setting/Debug")]
    public static void ChangeDebug()
    {
        Camera.main.GetComponent<Main>().UseLocalConfig = true;
    }

    /// <summary>
    /// 切换为正式状态
    /// </summary>
    [MenuItem("Titan/Setting/Formal")]
    public static void ChangeFormal()
    {
        Camera.main.GetComponent<Main>().UseLocalConfig = false;
    }

    /// <summary>
    /// 导出apk，修改设置导出工程
    /// </summary>
    [MenuItem("Titan/Build/Build Android Debug")]
    public static void BuildAndroidDebug()
    {
        BuildPipeline.BuildPlayer(GetLevels(),AndroidPath+ PackageName + ".apk", BuildTarget.Android, BuildOptions.Development);
    }

    /// <summary>
    /// 导出ios工程用包 包会比较小
    /// </summary>
    [MenuItem("Titan/Build/Build Test IOS")]
    public static void BuildTestIOS()
    {
        BuildPipeline.BuildPlayer(GetLevels(),IosPath+ PackageName, BuildTarget.iOS, BuildOptions.SymlinkLibraries);
    }

    /// <summary>
    /// 导出ios工程用包 包体大
    /// </summary>
    [MenuItem("Titan/Build/Build IOS Debug")]
    public static void BuildIOSDebug()
    {
        
        BuildPipeline.BuildPlayer(GetLevels(), IosPath+ PackageName, BuildTarget.iOS, BuildOptions.Development);
    }

    /// <summary>
    /// 获取所有场景
    /// </summary>
    public static string[] GetLevels()
    {
        List<string> scenePaths = new List<string>();
        foreach (var scene in EditorBuildSettings.scenes)
        {
            if (scene.enabled)
                scenePaths.Add(scene.path);
        }
        return scenePaths.ToArray();
    }

    /// <summary>
    /// 返回当前进程命令行参数字符串数组
    /// </summary>
    /// <param name="argName"></param>
    /// <returns></returns>
    public static string GetArgValue(string argName)
    {
        foreach (string arg in System.Environment.GetCommandLineArgs())
        {
            if (arg.StartsWith(argName))
            {
                return arg.Split("-"[0])[1];
            }
        }
        return "test";
    }
}

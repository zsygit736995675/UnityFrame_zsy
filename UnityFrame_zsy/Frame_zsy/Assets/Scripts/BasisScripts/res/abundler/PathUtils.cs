using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 路径工具
/// </summary>
public class PathUtils  {

    public const string bundleFilePathRoot = "/res/buildRes/";//打包资源根路径
    public const string AssetBundlesOutPutPath = "Assets/StreamingAssets";//Ab包保存跟目录
    public static string tablePath = "/exp/exp_bytes/";//表格路径



    /// <summary>
    /// 获取资源路径
    /// </summary>
    public static List<string> GetBundlePath(string path)
    {
        List<string> paths = new List<string>();
        paths.Add(Application.dataPath + bundleFilePathRoot + path);
        return paths;
    }

    /// <summary>
    /// 获取资源保存路径
    /// </summary>
    public static string GetAssetOutPath(int target)
    {
        string path = "";
        switch (target)
        {
            case 5://win
                path = AssetBundlesOutPutPath + "/Android/";
                break;

            case 9://ios
                path = AssetBundlesOutPutPath + "/IOS/";
                break;
         
            case 13://android
                path = AssetBundlesOutPutPath + "/Android/";
                break;
        }
        return path;
    }

    /// <summary>
    /// 获取平台名称
    /// </summary>
    public static string GetPlatformName(int target)
    {
        string name = "";
        switch (target)
        {
            case 19://win
                name =   "Android";
                break;

            case 9://ios
                name =   "IOS";
                break;

            case 13://android
                name =  "Android";
                break;
        }
        return name;
    }


    /// <summary>
    /// 不同平台下StreamingAssets的路径
    /// </summary>
    public static readonly string dataPath =
#if UNITY_ANDROID && !UNITY_EDITOR
		"jar:file://" + Application.dataPath + "!/assets/Android/";
#elif UNITY_IOS && !UNITY_EDITOR
		"file://" + Application.dataPath + "/Raw/IOS/";
#elif UNITY_STANDALONE_WIN
        "file://" + Application.dataPath + "/StreamingAssets/Windows/";
#else
        "file://" + Application.dataPath + "/StreamingAssets/Windows/";
#endif



    /// <summary>
    /// 缓存目录名
    /// </summary>
    public static readonly string CachePath = Application.persistentDataPath + "/WebCache";

    /// <summary>
    /// 图片保存目录
    /// </summary>
    public static readonly string WebImageSavePath=CachePath+"/ImageCache/";








}

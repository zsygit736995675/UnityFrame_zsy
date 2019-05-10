using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 路径工具
/// </summary>
public class PathUtils  {

    public const string bundleFilePathRoot = "/res/buildRes/";//打包资源根路径
    public const string AssetBundlesOutPutPath = "Assets/StreamingAssets";//Ab打包保存跟目录
    public static string tablePath = "/exp/exp_bytes/";//表格路径
    public static string persiPath = Application.persistentDataPath;//缓存地址，有可能会在回调中使用，所以先保存一下，只能在主线程使用

    /// <summary>
    /// 版本管理文件url
    /// </summary>
    public static readonly string versionUrl = "http://u3d-model.oss-cn-beijing.aliyuncs.com/model/Res/Ver3/z_version.txt";

    /// <summary>
    /// 获取资源保存路径，打包时保存路径
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
    /// 获取平台名称，打包时用，传平台枚举值
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
    /// assetbundle包本地存放位置
    /// </summary>
    public static  string ABPath
    {
        get
        {
            string path = persiPath + "/WebCache/ResCache/";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
           return path;
        }
    }

    /// <summary>
    /// assetbundle包本地存放位置
    /// </summary>
    public static string LoadABPath
    {
        get
        {
            string path =  persiPath + "/WebCache/ResCache/";
            path = "file:///" + path;
            return path;
        }
    }

    /// <summary>
    /// 网络图片保存目录
    /// </summary>
    public static  string WebImageSavePath
    {
        get
        {
            string path= persiPath + "/WebCache/ImageCache/"; 
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }
    }

    /// <summary>
    /// 平台名称，下载时只区分安卓和ios，pc使用安卓的资源
    /// </summary>
    public static readonly string platName =
#if UNITY_IOS 
		 "IOS";
#else
       "Android";
#endif


}

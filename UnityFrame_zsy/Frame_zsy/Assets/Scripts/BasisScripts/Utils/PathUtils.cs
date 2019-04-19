using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 路径工具
/// </summary>
public class PathUtils  {

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

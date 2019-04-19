using System;
using UnityEngine;
using System.IO;

/// <summary>
/// 表格基类
/// </summary>
public class BaseConfig
{
    public static bool isUseLocalTable = true;
    //加载完后mGameConfig就会置为空，mGameConfig是个临时变量//
    static public AssetBundle mGameConfig = null;

    static protected void LogError(string text)
    {
        Debug.LogError(text);
    }

    static protected void LogWarning(string text)
    {
        Debug.LogWarning(text);
    }

    static protected byte[] GetAsset(string name)
    {
        if (isUseLocalTable)
        {
            string file = Application.dataPath + "/exp/exp_bytes/" + name;
            FileStream fs = new FileStream(file, FileMode.Open);
            long length = fs.Length;
            byte[] data = new byte[length];
            fs.Read(data, 0, (int)length);
            fs.Close();
            return data;
        }
        return (mGameConfig.LoadAsset(name) as TextAsset).bytes;
    }
}


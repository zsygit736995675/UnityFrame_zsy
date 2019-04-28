using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// obj工具 提供查找，删除等功能
/// </summary>
public class ObjUtils  {


    /// <summary>
    /// 获取对象路径
    /// </summary>
    /// <param name="go"></param>
    /// <param name="root"></param>
    /// <returns></returns>
    public static string GetPath(GameObject go, GameObject root = null)
    {
        string path = go.name;
        Transform parent = go.transform.parent;
        while (parent != null && parent.gameObject != root)
        {
            path = parent.gameObject.name + "/" + path;
            parent = parent.parent;
        }
        return path;
    }

    /// <summary>
    /// 查找子物体（递归查找）  
    /// </summary> 
    /// <param name="trans">父物体</param>
    /// <param name="goName">子物体的名称</param>
    /// <returns>找到的相应子物体</returns>
    public static Transform FindChild(Transform trans, string goName)
    {
        Transform child = trans.Find(goName);
        if (child != null)
        {
            return child;
        }

        Transform go = null;
        for (int i = 0; i < trans.childCount; i++)
        {
            child = trans.GetChild(i);
            go = FindChild(child, goName);
            if (go != null)
            {
                return go;
            }  
        }
        return null;
    }

    /// <summary>
    ///  去掉所有UIPrefab上的脚本
    /// </summary>
    public static void CleanScripts(GameObject go)
    {
        if (go == null)
            return;

        foreach (Transform child in go.transform)
        {
            CleanScripts(child.gameObject);
        }
    }

    /// <summary>
    /// 查找子物体（递归查找）  where T : UnityEngine.Object
    /// </summary> 
    /// <param name="trans">父物体</param>
    /// <param name="goName">子物体的名称</param>
    /// <returns>返回相应对象</returns>
    public static T FindChild<T>(Transform trans, string goName) where T : UnityEngine.Object
    {
        Transform child = trans.Find(goName);
        if (child != null)
        {
            return child.GetComponent<T>();
        }

        Transform go = null;
        for (int i = 0; i < trans.childCount; i++)
        {
            child = trans.GetChild(i);
            go = FindChild(child, goName);
            if (go != null)
            {
                return go.GetComponent<T>();
            }
        }
        return null;
    }



}

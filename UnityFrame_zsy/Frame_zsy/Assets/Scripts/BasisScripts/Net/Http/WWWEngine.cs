using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using ClientCore;

/// <summary>
/// 基于www封装，通讯下载
/// </summary>
public class WWWEngine : SingletonObject<WWWEngine> {

    private static string ABundlePath = "/AbundleCache/";

    /// <summary>
    /// 初始化 下载路径
    /// </summary>
    protected override void Spawn()
    {
        if (!Directory.Exists(PathUtils.WebImageSavePath))
        {
            Directory.CreateDirectory(PathUtils.WebImageSavePath);
        }
    }

    /// <summary>
    /// 静态设置
    /// </summary>
    public static  void SetGameAsyncImage(Image image,string imagename,string url)
    {
        if (url != null)
        {
            WWWEngine.Ins.SetAsyncImage(url, image);
        }
        else
        {
            Logger.LogError("SetAsyncImage url null");
        }
    }

    /// <summary>
    /// 异步设置图片
    /// </summary>
    private void SetAsyncImage(string url,Image image)
    {
        //是否第一次加载
        if (!File.Exists(PathUtils.WebImageSavePath+ url.GetHashCode()))
        {
            StartCoroutine(LoadWebImage(url,image));
        }
        else
        {
            StartCoroutine(LoadLocalImage(url,image));
        }
    }

    /// <summary>
    /// 加载网图
    /// </summary>
    IEnumerator LoadWebImage(string url,Image image)
    {
        if (url != "" || image != null)
        {
            Logger.Log(url);
            WWW www = new WWW(url);
            yield return www;

            if (www.error == null)
            {
                try
                {
                    Texture2D tex2d = www.texture;
                    if (tex2d != null)
                    {
                        //保存图片
                        byte[] pngData = tex2d.EncodeToPNG();
                        File.WriteAllBytes(PathUtils.WebImageSavePath + url.GetHashCode(), pngData);
                        Sprite m_sprite = Sprite.Create(tex2d, new Rect(0, 0, tex2d.width, tex2d.height), Vector2.zero);
                        image.sprite = m_sprite;
                    }
                    else
                    {
                        Logger.LogError("Texture2D null");
                    }
                }
                catch (System.Exception error)
                {
                    Logger.LogError(error);
                    throw error;
                }
            }
            else
            {
                Logger.LogError(www.error);
            }
        }
    }

    public static IEnumerator LoadModel(string url,string name,Action<WWW> process)
    {
         yield return WWWEngine.Ins.DownFile(url, name, process);
    }


    static System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
    /// <summary>
    /// 下载文件
    /// </summary>
    public IEnumerator DownFile(string url, string savePath, Action<WWW> process)
    {
        savePath = PathUtils.CachePath + ABundlePath + savePath;
        FileInfo file = new FileInfo(savePath);
        stopWatch.Start();
        UnityEngine.Debug.Log("Start:" + Time.realtimeSinceStartup);
        WWW www = new WWW(url);
        while (!www.isDone)
        {
            yield return 0;
            if (process != null)
                process(www);
        }
        yield return www;
        if (www.isDone)
        {
            byte[] bytes = www.bytes;
            CreatFile(savePath, bytes,www);
            if (process != null)
                process(www);
        }
    }

    /// <summary>
    /// 创建文件
    /// </summary>
    /// <param name="bytes"></param>
    public void CreatFile(string savePath, byte[] bytes,WWW www)
    {
        FileStream fs = new FileStream(savePath, FileMode.Append);
        BinaryWriter bw = new BinaryWriter(fs);
        fs.Write(bytes, 0, bytes.Length);
        fs.Flush();     //流会缓冲，此行代码指示流不要缓冲数据，立即写入到文件。
        fs.Close();     //关闭流并释放所有资源，同时将缓冲区的没有写入的数据，写入然后再关闭。
        fs.Dispose();   //释放流
        www.Dispose();

        stopWatch.Stop();
        UnityEngine.Debug.Log("下载完成,耗时:" + stopWatch.ElapsedMilliseconds);
        UnityEngine.Debug.Log("End:" + Time.realtimeSinceStartup);
    }

    /// <summary>
    /// 从本地加载图片
    /// </summary>
    IEnumerator LoadLocalImage(string url, Image image)
    {
        string filePath = "file:///" + PathUtils.WebImageSavePath+ url.GetHashCode();
        if (url != "" || image != null)
        {
            WWW www = new WWW(filePath);
            yield return www;

            if (www.error == null)
            {
                Texture2D texture = www.texture;
                if (texture != null)
                {
                    Sprite m_sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
                    image.sprite = m_sprite;
                }
                else
                {
                    Logger.LogError("Texture2D null");
                }
            }
            else
            {
                Logger.LogError(www.error);
            }
        }
    }



}

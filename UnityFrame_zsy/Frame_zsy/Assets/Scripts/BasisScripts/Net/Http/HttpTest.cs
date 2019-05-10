using System;
using System.IO;
using System.Net;
using System.Threading;
using UnityEngine;

/// <summary>
/// Http下载
/// </summary>
public class MyHttp
{
    /// <summary>
    /// 下载进度(百分比)
    /// </summary>
    public static float progress { get; private set; }

    private static bool isStop;

    private static Thread thread;

    /// <summary>
    /// 下载文件(断点续传)
    /// </summary>
    /// <param name="_url">下载地址</param>
    /// <param name="_filePath">本地文件存储目录</param>
    public static void Download(string _url, string _fileDirectory, string _fileName,Action endCallback,Action<string , float,long, long > progressCallback)
    {
        string Temporarysuffix = ".bmp";

        isStop = false;

        thread = new Thread(delegate ()
        {

            if (!Directory.Exists(_fileDirectory))
            {
                Directory.CreateDirectory(_fileDirectory);
            }

            string filePath = _fileDirectory + _fileName + Temporarysuffix;
            string realFilePath = _fileDirectory + _fileName;

            if (!File.Exists(filePath))
            {
                File.Create(filePath).Dispose();
            }

            FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);

            long fileLength = fileStream.Length;

            long totalLength =  GetLength(_url);

            if (fileLength < totalLength)
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(_url);
                request.AddRange((int)fileLength);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                fileStream.Seek(fileLength, SeekOrigin.Begin);
                Stream httpStream = response.GetResponseStream();
                byte[] buffer = new byte[1024];
                int length = httpStream.Read(buffer, 0, buffer.Length);
                while (length > 0)
                {
                    if (isStop)
                    {
                         break;
                    }
                    fileStream.Write(buffer, 0, length);
                    fileLength += length;
                    progress = (float)fileLength / totalLength * 100;
                    fileStream.Flush();
                    length = httpStream.Read(buffer, 0, buffer.Length);

                    if (progressCallback != null)
                    {
                        progressCallback.Invoke(_fileName,progress,fileLength,totalLength);
                    }
                }
                httpStream.Close();
                httpStream.Dispose();
            }

            fileStream.Close();
            fileStream.Dispose();

            if (fileLength >= totalLength)
            {
                Logger.Log(_fileName+" DownLoad Fnish");
              
                progress = 100;

                if (endCallback != null)
                {
                    endCallback();
                }

                File.Move(filePath, realFilePath);
            }
        })
        {
            IsBackground = true
        };
        thread.Start();
    }

    /// <summary>
    /// 关闭线程
    /// </summary>
    public void Close()
    {
        Debug.Log("Close");
        isStop = true;
    }


    public float GetCurrentProgress(string _url, string _fileDirectory, string _fileName)
    {
        string filePath = _fileDirectory + "/" + _fileName + ".bmp";
        string realFilePath = _fileDirectory + "/" + _fileName + ".assetbundle";
        string mUrl = _url + _fileName + ".assetbundle";
        float currentProgress = 0;
        if (File.Exists(realFilePath))
        {
            Debug.Log("cunzai");
            currentProgress = 100.0f;
        }
        else
        {
            FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read);
            long fileLength = fileStream.Length;
            long totalLength = GetLength(mUrl);
            Debug.Log(fileLength + "  " + totalLength);
            currentProgress = (float)fileLength / totalLength * 100;
            Debug.Log(currentProgress);
        }
        return currentProgress;
    }

    static long  GetLength(string _fileUrl)
    {
        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(_fileUrl);
        request.Method = "HEAD";
        HttpWebResponse res = (HttpWebResponse)request.GetResponse();
        return res.ContentLength;
    }

}
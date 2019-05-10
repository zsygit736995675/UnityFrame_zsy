using UnityEngine;
using System.Collections;
using System.Threading;
using System.IO;
using System.Net;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

/// <summary>
/// 通过http下载资源
/// </summary>
public class HttpDownLoad  : BaseClass<HttpDownLoad>{


	//下载进度
	public float progress{get; private set;}

	//涉及子线程要注意,Unity关闭的时候子线程不会关闭，所以要有一个标识
	private bool isStop;

	//子线程负责下载，否则会阻塞主线程，Unity界面会卡主
	private Thread thread;

	//表示下载是否完成
	public bool isDone{get; private set;}

    const int ReadWriteTimeOut = 2 * 1000;//超时等待时间

    const int TimeOutWait = 5 * 1000;//超时等待时间

    /// <summary>
    /// 下载方法(断点续传)
    /// </summary>
    /// <param name="url">URL下载地址</param>
    /// <param name="savePath">Save path保存路径</param>
    /// <param name="callBack">Call back回调函数</param>
    public void DownLoad(string url, string savePath, string fileName, long totalLength,Action callBack, System.Threading.ThreadPriority threadPriority = System.Threading.ThreadPriority.Normal)
	{
        Logger.Log("loadurl:"+url+ "  fileName:" + fileName+"  savePath:" + savePath+ "  totalLength:" + totalLength);
     
        isStop = false;
        System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();

        //开启子线程下载,使用匿名方法
        thread = new Thread(delegate() {

            stopWatch.Start();

            if (!Directory.Exists(savePath))
			{
				Directory.CreateDirectory(savePath);
			}

            string filePath = savePath + fileName;

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Create(filePath).Dispose();
            }
           
			//使用流操作文件
			FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);

			//获取文件现在的长度
			long fileLength = fs.Length;

            //如果没下载完
            if (fileLength < totalLength)
			{
                //断点续传核心，设置本地文件流的起始位置
                fs.Seek(fileLength, SeekOrigin.Begin);

				HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
                request.ReadWriteTimeout = ReadWriteTimeOut;
                request.Timeout = TimeOutWait;
          
                //断点续传核心，设置远程访问文件流的起始位置
                request.AddRange((int)fileLength);

                Stream  stream = request.GetResponse().GetResponseStream();
				byte[] buffer = new byte[2048];

				//使用流读取内容到buffer中
				//注意方法返回值代表读取的实际长度,并不是buffer有多大，stream就会读进去多少
				int length = stream.Read(buffer, 0, buffer.Length);
             
                while (length > 0)
				{
					//如果Unity客户端关闭，停止下载
					if(isStop) break;
					//将内容再写入本地文件中
					fs.Write(buffer, 0, length);
					//计算进度
					fileLength += length;
					progress = (float)fileLength / (float)totalLength;
					
					//类似尾递归
					length = stream.Read(buffer, 0, buffer.Length);
                    //Debug.LogFormat("<color=red>文件:{0} 已下载{1}M，剩余{2}M</color>", fileName, fileLength / 1024 / 1024, (totalLength - fileLength) / 1024 / 1024);
                }
				stream.Close();
				stream.Dispose();
            }
            else
			{
				progress = 1;
            }
            stopWatch.Stop();
            Debug.Log(fileName+"耗时: " + stopWatch.ElapsedMilliseconds);
            fs.Close();
			fs.Dispose();
			//如果下载完毕，执行回调
			if(progress >= 1)
			{
                isDone = true;
                UnityEngine.Debug.Log(fileName + "  download finished");
                if (callBack != null) callBack();
                thread.Abort();
            }
           
		});
		//开启子线程
		thread.IsBackground = true;
        thread.Priority = threadPriority;
		thread.Start();
    }

    public void Close()
	{
		isStop = true;
	}

}

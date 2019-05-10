using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;


/// <summary>
/// 版本管理器
/// </summary>
public class VersionManager :  BaseClass<VersionManager>{

    /// <summary>
    /// 版本号
    /// </summary>
    private int version;
    public int Version
    {
        get { return version; }
    }

    /// <summary>
    /// 资源url
    /// </summary>
    private string url = "";
    public string URL
    {
        get { return url; }
    }

    /// <summary>
    /// ab包配置文件
    /// </summary>
    private string mapName = "";
    public string MapName
    {
        get { return mapName; }
    }

    /// <summary>
    /// 下载结束
    /// </summary>
    private Utils.VoidDelegate LoadCallback;

    /// <summary>
    /// 配置文件
    /// </summary>
    private Dictionary<string, List<string>> mapDic = new Dictionary<string, List<string>>();

    /// <summary>
    /// 需要更新的资源
    /// </summary>
    private Dictionary<string, List<string>> needUpDic = new Dictionary<string, List<string>>();

    /// <summary>
    /// 资源名称
    /// </summary>
    private List<string> keys = new List<string>();

    /// <summary>
    /// 下载计数
    /// </summary>
    private int loadCount = 0;

    /// <summary>
    /// 检测版本更新
    /// </summary>
    public void Update(int ver,Utils.VoidDelegate callback)
    {
        version = ver;
        LoadCallback = callback;
        Coroutiner.Start(LoadVersion());
    }

    /// <summary>
    /// 下载版本文件
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadVersion()
    {
        Logger.Log("LoadVersion url:"+PathUtils.versionUrl);
        WWW www = new WWW(PathUtils.versionUrl);

        yield return www;

        if (string .IsNullOrEmpty(www.error) && www.isDone)
        {
            JArray jsonArr = (JArray)JsonConvert.DeserializeObject(www.text);
            foreach (JObject json in jsonArr)
            {
                string webVersion = json["localversion"].ToString();

                if (int.Parse(webVersion) == Version)
                {
                    url = json["url"].ToString();
                    mapName = json["map"].ToString();
                    if (!string.IsNullOrEmpty(mapName))
                    {
                        Coroutiner.Start(DownMap());
                    }
                    else
                    {
                        Logger.LogError("LoadVersion error: mapName null!");
                    }
                    break;
                }
            }
        }
        else
        {
            Logger.LogError("LoadVersion error:"+www.error);
        }
    }

    /// <summary>
    /// 下载map
    /// </summary>
    IEnumerator  DownMap()
    {
        string mapUrl = url + PathUtils.platName + "/" + mapName;
        WWW www = new WWW(mapUrl);
        Logger.Log("DownMap url:" + mapUrl);
        yield return www;

        if (string.IsNullOrEmpty(www.error) && www.isDone)
        {
            
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(www.text);
            XmlElement xmlRoot = xmlDoc.DocumentElement;
           
            foreach (XmlNode node in xmlRoot.ChildNodes)
            {
                List<string> nodeList = new List<string>();
                if (!(node is XmlElement))
                {
                    continue;
                }
      
                string file = (node as XmlElement).GetAttribute("FileName");
                string md5 = (node as XmlElement).GetAttribute("MD5");
                string size = (node as XmlElement).GetAttribute("Size");
                nodeList.Add(md5);
                nodeList.Add(size);

                if (mapDic.ContainsKey(file) == false)
                {
                    mapDic.Add(file, nodeList);
                }
            }
            ResVersionCheck();
        }
        else
        {
            Logger.LogError("DownMap error:" + www.error);
        }
    }

    /// <summary>
    /// 资源版本校验 首先对比版本文件，再对比资源文件，md5或文件大小不一致则需要更新
    /// </summary>
    void ResVersionCheck()
    {
        needUpDic.Clear();
        keys.Clear();
        string mapPath = PathUtils.ABPath + "/VersionMD5.xml";
        if (File.Exists(mapPath))
        {
            Dictionary<string, List<string>> tempDic = FileUtils.ReadMD5File(mapPath);
            List<string> outValue = new List<string>();
          
            foreach (KeyValuePair<string, List<string>> item in mapDic)
            {
                if (outValue != null)
                {
                    outValue.Clear();
                }

                if (tempDic.TryGetValue(item.Key, out outValue))
                {
                    if (outValue[0] != item.Value[0] || outValue[1] != item.Value[1])
                    {
                        //md5值或者文件大小不一致更新
                        keys.Add(item.Key);
                        needUpDic.Add(item.Key, item.Value);
                        continue;
                    }
                }
                else
                {
                    //map列表对不上 更新
                    keys.Add(item.Key);
                    needUpDic.Add(item.Key, item.Value);
                    continue;
                }

                if (!File.Exists(PathUtils.ABPath  + item.Key) && !needUpDic.ContainsKey(item.Key))
                {
                    //文件不存在更新
                    keys.Add(item.Key);
                    needUpDic.Add(item.Key, item.Value);
                }
                else
                {
                    FileStream file = new FileStream(PathUtils.ABPath + "/" + item.Key, FileMode.Open, FileAccess.Read, FileShare.Read);
                    if (file.Length != long.Parse(item.Value[1]))
                    {
                        // 文件大小不一致
                        keys.Add(item.Key);
                        needUpDic.Add(item.Key, item.Value);
                    }
                }
            }
        }
        else
        {
            //更新所有
            foreach (var item in mapDic)
            {
                keys.Add(item.Key);
            }
            needUpDic = mapDic;
        }
        DownLoadRes();
    }

    /// <summary>
    /// 更新资源
    /// </summary>
    void DownLoadRes()
    {
        Logger.Log("DownLoadRes : needupdate  " + needUpDic.Count + " file");

        if (needUpDic == null || needUpDic.Count < 1)
        {
            if (LoadCallback != null)
            {
                LoadCallback();
            }
            return;
        }

        try
        {
            //更新map文件
            XmlDocument XmlDoc = new XmlDocument();
            XmlElement XmlRoot = XmlDoc.CreateElement("Files");
            XmlDoc.AppendChild(XmlRoot);
            foreach (KeyValuePair<string, List<string>> pair in mapDic)
            {
                XmlElement xmlElem = XmlDoc.CreateElement("File");
                XmlRoot.AppendChild(xmlElem);
                xmlElem.SetAttribute("FileName", pair.Key);
                xmlElem.SetAttribute("MD5", pair.Value[0]);
                xmlElem.SetAttribute("Size", pair.Value[1]);
            }
            string md5Path = PathUtils.ABPath + "/VersionMD5.xml";
            Logger.Log("XmlDoc.Save:"+ md5Path);
            XmlDoc.Save(md5Path);
         
            XmlDoc = null;
            loadCount = 0;
        }
        catch (System.Exception e)
        {
            Logger.LogError(e.ToString());
            throw;
        }

        RecursiveDown();
    }

    /// <summary>
    /// 递归下载
    /// </summary>
    void RecursiveDown()
    {
        if (!(loadCount < keys.Count))
        {
            DownLoadEnd();
            return;
        }

        string key = keys[loadCount];
        Logger.Log("RecursiveDown key :" + key );

        if (string.IsNullOrEmpty(key))
        {
            loadCount++;
            RecursiveDown();
            return;
        }

        List<string> item = null;
        if (needUpDic.TryGetValue(key, out item))
        {
            string loadUrl = url + "Android/" + key;
            loadCount++;
            MyHttp .Download(loadUrl, PathUtils.ABPath, key, RecursiveDown, ProgressrateCallBack);
            DownLoadStart(key);
        }
        else
        {
            loadCount++;
            RecursiveDown();
        }
    }

    /// <summary>
    /// 开始下载某文件
    /// </summary>
    void DownLoadStart(string fileName)
    {
        Logger.Log("DownLoadStart:"+fileName);
    }

    /// <summary>
    /// 更新结束
    /// </summary>
    void DownLoadEnd()
    {
        Logger.Log("DownLoadEnd");
        if (LoadCallback != null)
        {
            LoadCallback();
        }
    }

    /// <summary>
    /// 下载进度回调
    /// </summary>
    void ProgressrateCallBack(string fileName,float pro,long culength, long tolength)
    {
        //Logger.Log("ProgressrateCallBack fileName:"+fileName+" pro:"+pro+ " culength:" + culength + " tolength:" + tolength);
    }


}

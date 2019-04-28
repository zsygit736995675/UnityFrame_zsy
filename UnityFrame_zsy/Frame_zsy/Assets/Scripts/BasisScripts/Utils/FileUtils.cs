using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Xml;
using UnityEditor;
using UnityEngine;

public class FileUtils  {

    /// <summary>
    ///  得到文件父文件夹的名字
    /// </summary>
    public static string GetParentFileName(string path, int parentIndex)
    {
        string[] filenames = path.Split('/');
        return filenames[filenames.Length - parentIndex];
    }

    /// <summary>
    /// 从路径中获取文件名称
    /// </summary>
    public static string GetFileName(string path)
    {
        int begin = path.LastIndexOf("/");
        if (begin < 0)
            begin = path.LastIndexOf("\\");
        int end = path.LastIndexOf(".");
        return path.Substring(begin + 1, end - begin - 1);
    }


    /// <summary>
    /// 获取子文件名称，只获取第一层，取的是文件夹的名称
    /// </summary>
    public static List<string> getTopChildFilesName(string path)
    {
        List<string> list=new List<string>();
        if (Directory.Exists(path))
        {
            //获取文件信息
            DirectoryInfo direction = new DirectoryInfo(path);

            FileInfo[] files = direction.GetFiles("*", SearchOption.TopDirectoryOnly);

            string nameStr = "";
            for (int i = 0; i < files.Length; i++)
            {
                nameStr = files[i].Name;
                if (nameStr.EndsWith(".meta"))
                {
                    nameStr= nameStr.Replace(".meta","");
                }
                list.Add(nameStr);
            }
            return list;
        }
        else
        {
            Logger.LogError("getTopChildFilesName  path not Exists!");
            return null;
        }
    }


    /**
     * 获取文件md5
     */
    public static string getFileHash(string filePath)
    {
        try
        {
            FileStream fs = new FileStream(filePath, FileMode.Open);
            int len = (int)fs.Length;
            byte[] data = new byte[len];
            fs.Read(data, 0, len);
            fs.Close();
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(data);
            string fileMD5 = "";
            foreach (byte b in result)
            {
                fileMD5 += Convert.ToString(b, 16);
            }
            return fileMD5;
        }
        catch (FileNotFoundException e)
        {
            Console.WriteLine(e.Message);
            return "";
        }
    }

    /// <summary>
    /// 指定文件取md5并保存xml
    /// </summary>
    public static void CreatMD5XML(string path)
    {
        Dictionary<string, List<string>> DicFileMD5 = new Dictionary<string, List<string>>();
        MD5CryptoServiceProvider md5Generator = new MD5CryptoServiceProvider();

        //需要创建MD5文件的文件夹路径
        string dir = System.IO.Path.Combine(Application.dataPath, path);
        //MD5保存路径
        string savePath = System.IO.Path.Combine(Application.dataPath, path);

        foreach (string filePath in Directory.GetFiles(dir))
        {

            if (filePath.Contains(".meta") || filePath.Contains("VersionMD5") || filePath.Contains("manifest") || filePath.Contains(".xml"))
                continue;

            FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            byte[] hash = md5Generator.ComputeHash(file);
            List<string> tempList = new List<string>();
            string strMD5 = System.BitConverter.ToString(hash);
            string size = file.Length.ToString();
            tempList.Add(strMD5);
            tempList.Add(size);

            file.Close();

            string key = filePath.Substring(dir.Length + 1, filePath.Length - dir.Length - 1);

            if (DicFileMD5.ContainsKey(key) == false)
                DicFileMD5.Add(key, tempList);
            else
                Debug.LogWarning("<Two File has the same name> name = " + filePath);
        }

        if (Directory.Exists(savePath) == false)
        {
            Directory.CreateDirectory(savePath);
        }

        XmlDocument XmlDoc = new XmlDocument();
        XmlElement XmlRoot = XmlDoc.CreateElement("Files");
        XmlDoc.AppendChild(XmlRoot);
        foreach (KeyValuePair<string, List<string>> pair in DicFileMD5)
        {
            XmlElement xmlElem = XmlDoc.CreateElement("File");
            XmlRoot.AppendChild(xmlElem);
            xmlElem.SetAttribute("FileName", pair.Key);
            xmlElem.SetAttribute("MD5", pair.Value[0]);
            xmlElem.SetAttribute("Size", pair.Value[1]);
        }

        XmlDoc.Save(savePath + "/VersionMD5.xml");
        XmlDoc = null;
    }

    /// <summary>
    /// 读xml
    /// </summary>
    /// <param name="fileName"></param>
    static Dictionary<string, List<string>> ReadMD5File(string fileName)
    {
        Dictionary<string, List<string>> DicMD5 = new Dictionary<string, List<string>>();
        // 如果文件不存在，则直接返回
        if (System.IO.File.Exists(fileName) == false)
            return DicMD5;

        XmlDocument XmlDoc = new XmlDocument();
        XmlDoc.Load(fileName);
        XmlElement XmlRoot = XmlDoc.DocumentElement;

        foreach (XmlNode node in XmlRoot.ChildNodes)
        {
            if ((node is XmlElement) == false)
            {
                continue;
            }

            List<string> tempList = new List<string>();
            string file = (node as XmlElement).GetAttribute("FileName");
            string md5 = (node as XmlElement).GetAttribute("MD5");
            string size = (node as XmlElement).GetAttribute("Size");
            tempList.Add(md5);
            tempList.Add(size);

            if (DicMD5.ContainsKey(file) == false)
            {
                DicMD5.Add(file, tempList);
            }
        }

        XmlRoot = null;
        XmlDoc = null;
        return DicMD5;
    }


}

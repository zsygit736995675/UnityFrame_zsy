using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
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



}

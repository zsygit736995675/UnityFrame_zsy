using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// 字符串工具类
/// </summary>
public class StrUtils  {




    /// <summary>
    /// Base64解密
    /// </summary>
    /// <param name="result">待解密的密文</param>
    /// <returns>解密后的字符串</returns>
    public static string Base64Decode(string result)
    {
        string decode = string.Empty;
        byte[] bytes = System.Convert.FromBase64String(result);
        try
        {
            decode = Encoding.UTF8.GetString(bytes);
        }
        catch
        {
            decode = result;
        }
        return decode;
    }


    /// <summary>
    /// Base64加密
    /// </summary>
    /// <param name="result">明文</param>
    /// <returns>密文</returns>
    public static string Base64Encode(string result)
    {
        string decode = string.Empty;
        byte[] bytes = System.Text.Encoding.Default.GetBytes(result);
        decode = System.Convert.ToBase64String(bytes);
        try
        {
            decode = get_UTF8(decode);
        }
        catch
        {
            decode = result;
        }
        return decode;
    }

    /// <summary>
    /// 字符串转UTF-8   
    /// </summary>
    /// <param name="unicodeString"></param>
    /// <returns></returns>
    public static string get_UTF8(string unicodeString)
    {
        UTF8Encoding utf8 = new UTF8Encoding();
        System.Byte[] encodedBytes = utf8.GetBytes(unicodeString);
        System.String decodedString = utf8.GetString(encodedBytes);
        return decodedString;
    }

    /// <summary>
    /// 从表格获取字符串
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static string GetStr(int id)
    {
        StrConfig con = StrConfig.GetConfig(id);
        if (con != null)
        {
            return con.str;
        }
        else
        {
            Logger.LogError("GetStr error:"+"ID："+id);
            return "";
        }
    }




}

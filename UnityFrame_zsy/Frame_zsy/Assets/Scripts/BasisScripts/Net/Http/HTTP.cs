using UnityEngine;
using System;
using System.Collections;

using System.Collections.Generic;
using System.Text;

namespace ClientCore
{
    public enum HTTPSeverType
    {
        None = 0,
        Sever = 1,       //应用服务器
        GameSever = 2,   //游戏服务器
        ImageUrl=3,      //图片地址   
    }

    /// <summary>
    /// 
    /// </summary>
    public class HTTP : BaseClass<HTTP>
    {
       
        /// <summary>
        /// 创建一个异步HTTP Post请求
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="callback">Callback</param>
        public HTTPRequest HttpAsyncPost(HTTPSeverType type, string str, string json, Action<string> callback = null)
        {
            Logger.Log("HttpAsyncPost:"+" type:"+type.ToString()+" path:"+str+" json:"+json);
            string url = GetSeverUrl(type, str);
            Logger.Log(url+json);

            HTTPRequest client = new HTTPRequest(url, "POST", 5000, (HTTPResponse response) =>
            {
                if (null != callback)
                {
                    Logger.Log("HttpAsyncPost Callback:"+" type:" + type.ToString() + " path:" + str + " json:" + json+ " response:"+ response.GetResponseText());
                    callback(response.GetResponseText());
                    callback = null;
                }
            });
            client.SetPostData(json);
            client.Start();
            return client;
        }

        /// <summary>
        /// 创建一个异步HTTP Get请求
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="callback">Callback</param>
        public HTTPRequest HttpAsyncGet(HTTPSeverType type, string str, Action<string> callback = null)
        {
            string url = GetSeverUrl(type, str);
            Logger.Log(url);

            HTTPRequest client = new HTTPRequest(url, "Get", 5000, (HTTPResponse response) =>
            {
                if (null != callback)
                {
                    callback(response.GetResponseText());
                }
            });
            client.Start();
            return client;
        }

        /// <summary>
        /// 同步POST
        /// </summary>
        /// <returns></returns>
        public string HttpPost(HTTPSeverType type, string str, string json)
        {
            string url = GetSeverUrl(type, str);
            Debug.Log(url);
            return new WebRequestSugar().HttpPost(url, json);
        }

        /// <summary>
        /// 同步GET
        /// </summary>
        /// <returns></returns>
        public string HttpGet(HTTPSeverType type, string str)
        {
            string url = GetSeverUrl(type, str);
            Debug.Log(url);
            return new WebRequestSugar().HttpGet(url);
        }

        public string GetSeverUrl(HTTPSeverType type, string str)
        {
            int confId = 0;
            IPConfig con = IPConfig.GetConfig(confId);
            StringBuilder sb = new StringBuilder();
            if (con.type == 0)
                sb.Append("http://");
            else if (con.type == 1)
                sb.Append("https://");
            sb.Append(con.ip);
            
            if (con.type == 0)
            {
                sb.Append(":");
                sb.Append(con.port);
            }
               
            sb.Append("/");
            if (!string.IsNullOrEmpty(con.url))
            {
                sb.Append(con.url);
                sb.Append("/");
            }
            sb.Append(str);
            return sb.ToString();
        }

    }
}
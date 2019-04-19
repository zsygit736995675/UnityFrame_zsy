using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    /// <summary>
    /// HTTP请求类型
    /// </summary>
    public enum HTTP_REQUEST_TYPE
    {
        /// <summary>
        /// GET请求
        /// </summary>
        Get,

        /// <summary>
        /// POST请求
        /// </summary>
        Post,
    }

    /// <summary>
    /// HTTP异步请求引擎
    /// </summary>
    public sealed class HttpEngine
    {
        #region Singleton

        /// <summary>
        /// 私有构造方法
        /// </summary>
        //private HttpEngine()
        //{
        //}

        /// <summary>
        /// 销毁单例
        /// </summary>
        //public override void Discard()
        //{
        //    if(_WWW != null)
        //    {
        //        _WWW.Dispose();
        //    }

        //    base.Discard();
        //}

        #endregion

        #region Send http request

        /// <summary>
        /// 请求是否正在处理中
        /// </summary>
        private bool _Processing = false;

        /// <summary>
        /// HTTP请求成功回调函数
        /// </summary>
		public delegate void SuccessCallBack(string response);

        /// <summary>
        /// 发送HTTP请求
        /// </summary>
        /// <returns>HTTP引擎状态</returns>
        /// <param name="type">HTTP请求类型</param>
        /// <param name="url">服务器URL地址</param>
        /// <param name="success">请求成功回调函数</param>
        /// <param name="data">需要传递的数据</param>
        /// <param name="autoMask">是否自动开启操作屏蔽遮罩层</param>
        /// <param name="timeOut">请求超时秒数</param>
        public void SendRequest(HTTP_REQUEST_TYPE type, string url, SuccessCallBack success, string data = "", bool autoMask = true, float timeOut = 5.0f)
        {
            if (type == HTTP_REQUEST_TYPE.Post && data == "")
            {
#if UNITY_EDITOR
                Debug.LogError("使用POST方式请求服务器，data数据不能为空！");
#endif
                return;
            }

            if (_Processing)
            {
#if UNITY_EDITOR
                Debug.LogError("HTTP引擎正在请求服务器！");
#endif
                return;
            }

            Coroutiner.Start(
                _SendRequestCallback(type, url, success, data, autoMask, timeOut)
            );
        }

        #endregion

        #region HTTP asynchronous request callback

        /// <summary>
        /// HTTP异步请求对象
        /// </summary>
        private WWW _WWW = null;

        /// <summary>
        /// 通过协同发送异步HTTP请求
        /// </summary>
        /// <returns>HTTP请求协同</returns>
        /// <param name="request">HTTP请求数据</param>
        private IEnumerator _SendRequestCallback(HTTP_REQUEST_TYPE type, string url, SuccessCallBack success, string data, bool autoMask, float timeOut)
        {
            _Processing = true;

            //if(autoMask)
            //{
            //    UIDispatcher.WaitMask(true);
            //}

#if UNITY_EDITOR
            string debug = "URL地址：" + url + "\n";
            debug += "数据：" + data + "\n";
            DateTime debugTime = DateTime.UtcNow;
#endif

            //判断请求类型，生成请求头
            if (type == HTTP_REQUEST_TYPE.Get)
            {
                _WWW = new WWW(url + (data != "" ? ("?" + data) : ""));
            }
            else
            {
                _WWW = new WWW(
                    url,
                    System.Text.UTF8Encoding.UTF8.GetBytes(data),
                    new Dictionary<string, string>() {
                        { "Content-Type", "application/json; charset=utf-8" }
                    }
                );
            }

            //设置HTTP请求过期时间
            //TimeClock.One.New(
            //    "HttpEngineTimeOut",
            //    timeOut,
            //    delegate
            //    {
            //        _WWW.Dispose();
            //    }
            //);

            yield return _WWW;

#if UNITY_EDITOR
            debug += "消耗时间：" + (DateTime.UtcNow - debugTime).TotalMilliseconds / 1000 + "秒\n";
#endif

            _Processing = false;

            //if (autoMask)
            //{
            //    UIDispatcher.WaitMask(false);
            //}

            bool haveError = false;
            bool isTimeOut = false;

            try
            {
                haveError = _WWW.error != null;
                //TimeClock.One.Cancel("HttpEngineTimeOut", false);
            }
            catch
            {
                //UIMessageBox.Alert(
                //    "请求超时，是否重试？",
                //    delegate
                //    {
                //        HttpEngine.One().SendRequest(
                //            type,
                //            url,
                //            success,
                //            data,
                //            autoMask,
                //            timeOut
                //        );
                //    },
                //    false,
                //    "网络错误"
                //);
                isTimeOut = true;
            }

            if (!isTimeOut)
            {
#if UNITY_EDITOR
                if (haveError)
                {
                    Debug.LogError(debug = "服务器错误" + _WWW.error + "！" + debug + "\n");
                }
                else
                {
                    Debug.Log(debug = "请求成功！" + debug + "服务器返回值：" + _WWW.text + "\n");
                }
#endif

                if (haveError)
                {
                    //UIMessageBox.Alert(
                    //    "通讯错误，是否重试？",
                    //    delegate
                    //    {
                    //        HttpEngine.One().SendRequest(
                    //            type,
                    //            url,
                    //            success,
                    //            data,
                    //            autoMask,
                    //            timeOut
                    //        );
                    //    },
                    //    false,
                    //    "网络错误"
                    //);
                    _WWW.Dispose();
                }
                else
                {
                    string serverData = _WWW.text;
                    _WWW.Dispose();
                    success(serverData);
                }
            }
#if UNITY_EDITOR
            else
            {
                Debug.LogError(debug = "服务器响应超时！" + debug);
            }
#endif
        }

        #endregion
    }

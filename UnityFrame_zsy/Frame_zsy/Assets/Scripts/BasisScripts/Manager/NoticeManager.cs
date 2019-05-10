using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;

using UnityEngine;

/// <summary>
/// 原生通讯管理
/// </summary>
public class NoticeManager : SingletonObject<NoticeManager>
    {
#if UNITY_IOS && !UNITY_EDITOR
		[DllImport("__Internal")]
		private static extern void _onMessageRecieved(string func, string param);
#elif UNITY_ANDROID && !UNITY_EDITOR
        private static void _onMessageRecieved(string func, string param)
        {
		    using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		    {
			    Debug.Log("AndroidJavaClass" + jc);
			    using( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
			    {
				    jo.Call("_onMessageRecieved", func, param);
			    }
		    }
        }
#else
        private static void _onMessageRecieved(string func, string param)
        {
            Logger.LogError("_onMessageRecieved: func:"+func+ "  param:"+param);
        }
#endif

        List<int> FunctionPool = new List<int>();

        protected override void Spawn()
        {
            base.Spawn();
            FunctionPool.Clear();
        }

        /// <summary>
        /// 初始化将所有原生通知注册
        /// </summary>
        public void Init()
        {
            for (int i = (int)EventDef.RepAction+1; i < (int)EventDef.RepEnd; i++)
            {
                RegisterHandler(i);
            }
        }

        void RegisterHandler(int key)
        {
            try
            {
                if (!FunctionPool.Contains(key))
                {
                    FunctionPool.Add(key);
                }
                else
                {
                    Logger.Log(string.Format("RegisterHandler Error, Key: {0}.", key));
                }
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
            }
        }

        static public void OnCallPhone(string param)
        {
            NoticeManager.Ins.SendMessage(NoticeDef.OpenFunc, param);
        }

        public void BtnSendMessage(string param)
        {
            SendMessage(NoticeDef.OpenFunc, param);
        }

        public void SendMessage(string func, string param)
        {
            _onMessageRecieved(func, param);
        }

        public void OnMessageRecieved(string param)
        {
            try
            {
                Logger.Log(param);
                JObject json = (JObject)JsonConvert.DeserializeObject(param);
                string key = "";
                JToken content = "";
                foreach (var token in json)
                {
                    key = token.Key;
                    content = token.Value;
                    break;
                }

                for (int i = (int)EventDef.RepAction + 1; i < (int)EventDef.RepEnd; i++)
                {
                    EventDef def = (EventDef)i;
                    if (def.ToString() == key)
                    {
                        if (FunctionPool.Contains(i))
                        {
                            App.EventMgrHelper.PushEventEx(def, content, null, -1, -1, null, null);
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                Logger.Log(e.ToString());
            }
        }

        void UnityRevieverFunc(string s)
        {
            OnMessageRecieved(s);
        }

}




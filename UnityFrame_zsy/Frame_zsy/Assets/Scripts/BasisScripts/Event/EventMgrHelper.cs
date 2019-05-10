using System;
using System.Collections.Generic;

    /// <summary>
    /// 事件注册 分发
    /// </summary>
    public class EventMgrHelper : SingletonObject<EventMgrHelper>
    {
        public bool isInit = false;

        private Dictionary<EventDef, List<EventHandler>> mDicHandler = new Dictionary<EventDef, List<EventHandler>>();
        private List<LogicEvent>[] paramArray = new List<LogicEvent>[] { new List<LogicEvent>(), new List<LogicEvent>() };
        private int mParamIndex = 0;
        private List<LogicEvent> mListEvent = null;

        /// <summary>
        /// 注册事件
        /// </summary>
        public void RegisterEventHandler(EventDef ed, EventHandler handler)
        {
            List<EventHandler> lh;
            if (mDicHandler.TryGetValue(ed, out lh))
            {
                lh.Add(handler);
            }
            else
            {
                lh = new List<EventHandler>();
                lh.Add(handler);

                mDicHandler.Add(ed, lh);
            }
        }

        /// <summary>
        /// 解绑事件
        /// </summary>
        public void UnRegisterEventHandler(EventDef ed, EventHandler handler)
        {
            List<EventHandler> lh;
            if (mDicHandler.TryGetValue(ed, out lh))
            {
                for (int i = 0; i < lh.Count; ++i)
                {
                    if (lh[i] == handler)
                    {
                        lh.RemoveAt(i);
                        return;
                    }
                }
            }
        }

        public void Init()
        {
            mListEvent = paramArray[mParamIndex];
            isInit = true;
        }

        private LogicEvent CreateLogicEvent()
        {
            return LogicEvent.CreateEvent();
        }

        public void PushEventWhenNotExist(EventDef ed)
        {
            if (!ExistEvent(ed))
                this.PushEvent(ed);
        }

        private bool ExistEvent(EventDef le)
        {
            for (int i = 0; i < mListEvent.Count; ++i)
            {
                if (mListEvent[i].Event == le)
                    return true;
            }
            return false;
        }


        // use EventMgrHelper.PushEvent instead
        public void PushEvent(EventDef ed, int intdata0 = -1, int intdata1 = -1, string strData0 = "", string strData1 = "", bool checkSameEvent = false, float floatdata0 = -1, float floatdata1 = -1)
        {
            if (checkSameEvent)
            {
                if (ExistEvent(ed))
                    return;
            }
            LogicEvent le = CreateLogicEvent();
            le.Event = ed;
            le.IntParam0 = intdata0;
            le.IntParam1 = intdata1;
            le.StrParam0 = strData0;
            le.StrParam1 = strData1;
            le.FloatParam0 = floatdata0;
            le.FloatParam1 = floatdata1;

            PushEvent(le);
        }

        // use EventMgrHelper.PushEvent instead
        public void PushEventEx(EventDef ed, System.Object object0, System.Object object1=null, int data0=-1, int data1=-1, string strData0="", string strData1="", bool checkSameEvent = false, float floatdata0 = -1, float floatdata1 = -1)
        {
            if (checkSameEvent)
            {
                if (ExistEvent(ed))
                    return;
            }
            LogicEvent le = CreateLogicEvent();
            le.Event = ed;

            le.Object0 = object0;
            le.Object1 = object1;
            le.IntParam0 = data0;
            le.IntParam1 = data1;
            le.StrParam0 = strData0;
            le.StrParam1 = strData1;
            le.FloatParam0 = floatdata0;
            le.FloatParam1 = floatdata1;

            PushEvent(le);
        }


        private void PushEvent(LogicEvent le)
        {
            mListEvent.Add(le);

            if (mListEvent.Count > 50)
            {
                int i = 0;
                ++i;
            }

        }

        public int GetCount()
        {
            return mListEvent.Count;
        }

        public void Update()
        {
            if (mListEvent == null)
            {
                mParamIndex = (mParamIndex + 1) % 2;
                mListEvent = paramArray[mParamIndex];
                UnityEngine.Debug.Log("paramArray:"+paramArray.Length) ;
            }

            if (mListEvent.Count == 0)
                return;

            List<LogicEvent> oldEvent = mListEvent;
            mParamIndex = (mParamIndex + 1) % 2;
            mListEvent = paramArray[mParamIndex];

            for (int i = 0; i < oldEvent.Count; ++i)
            {
                LogicEvent le = oldEvent[i];

                List<EventHandler> lh;
                if (!mDicHandler.TryGetValue(le.Event, out lh))
                {
                    string logText = "event = " + le.Event.ToString() + " not find Handler";
                    UnityEngine.Debug.Log(logText);
                    continue;
                }
                for (int j = 0; j < lh.Count; ++j)
                {
                    try  //暂时不纠结异常问题
                    {
                        lh[j].HandleEvent(le.Event, le);
                    }
                    catch (System.Exception e)
                    {
                        UnityEngine.Debug.Log(le.Event + " " + e.Message);
                    }
                }

                if (!le.Locked)
                {
                    LogicEvent.DestroyEvent(le);
                }
            }
            oldEvent.Clear();
        }
    }

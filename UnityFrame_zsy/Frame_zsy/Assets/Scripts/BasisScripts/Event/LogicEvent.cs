using System;
using System.Collections.Generic;

    /// <summary>
    /// 事件实体类，基于对象池
    /// </summary>
    public class LogicEvent
    {
        public const string NUMBER_MIN_STR = "MIN";
        public const string NUMBER_MAX_STR = "MAX";
        public const string NUMBER_DEFAULT_STR = "DEFAULT";
        private EventDef mEvent = EventDef.NONE;
        private int mIntParam0 = -1;
        private int mIntParam1 = -1;
        private float mFloatParam0 = -1;
        private float mFloatParam1 = -1;
        private string mStrParam0 = "";
        private string mStrParam1 = "";
        private System.Object mObject0;
        private System.Object mObject1;
        private bool mLocked = false;
        private Dictionary<string, int> mDicParam0 = new Dictionary<string, int>();

        static MemoryPool<LogicEvent> msEventPool = new MemoryPool<LogicEvent>(50);

        public static int mDebugCount = 0;

        public LogicEvent()
        {
            ++mDebugCount;
        }

        private void Reset()
        {
            mEvent = EventDef.NONE;
            mIntParam0 = -1;
            mIntParam1 = -1;
            mFloatParam0 = -1;
            mFloatParam1 = -1;
            mStrParam0 = "";
            mStrParam1 = "";
            mObject0 = null;
            mObject1 = null;
            mDicParam0.Clear();
        }

        static public LogicEvent CreateEvent()
        {
            LogicEvent le = msEventPool.Alloc();
            if (le != null)
            {
                return le;
            }

            return new LogicEvent();
        }

        static public void DestroyEvent(LogicEvent le)
        {
            le.Reset();
            msEventPool.Free(le);
        }

        ~LogicEvent()
        {
            --mDebugCount;
        }

        public EventDef Event
        {
            set { mEvent = value; }
            get { return mEvent; }
        }

        public bool Locked
        {
            set { mLocked = value; }
            get { return mLocked; }
        }

        public int IntParam0
        {
            set { mIntParam0 = value; }
            get { return mIntParam0; }
        }

        public int IntParam1
        {
            set { mIntParam1 = value; }
            get { return mIntParam1; }
        }

        public float FloatParam0
        {
            set { mFloatParam0 = value; }
            get { return mFloatParam0; }
        }
        public float FloatParam1
        {
            set { mFloatParam1 = value; }
            get { return mFloatParam1; }
        }

        public string StrParam0
        {
            set { mStrParam0 = value; }
            get { return mStrParam0; }
        }

        public string StrParam1
        {
            set { mStrParam1 = value; }
            get { return mStrParam1; }
        }

        public System.Object Object0
        {
            set { mObject0 = value; }
            get { return mObject0; }
        }

        public System.Object Object1
        {
            set { mObject1 = value; }
            get { return mObject1; }
        }

        public Dictionary<string, int> DicParam0
        {
            set { mDicParam0 = value; }
            get { return mDicParam0; }
        }
    }

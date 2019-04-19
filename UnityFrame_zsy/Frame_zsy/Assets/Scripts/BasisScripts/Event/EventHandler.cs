
    /// <summary>
    /// 事件基类
    /// </summary>
    public class EventHandler
    {
      
        public delegate void _HandleEvent(EventDef ev, LogicEvent le);

        private _HandleEvent mhandler = null;

        public void RegisterEvent(EventDef ev)
        {
            App.EventMgrHelper.RegisterEventHandler(ev, this);
        }

        public void UnRegisterEvent(EventDef ev)
        {
            App.EventMgrHelper.UnRegisterEventHandler(ev, this);
        }

        public virtual void HandleEvent(EventDef ev, LogicEvent le)
        {
            if (mhandler != null)
            {
                mhandler(ev, le);
            }
        }

        public _HandleEvent Handler
        {
            set { mhandler = value; }
        }
    }

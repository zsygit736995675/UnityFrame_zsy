using System;
using PwNgs;

	public delegate void _OnTimer();

    public enum TimerID
    {
        TIMER_NONE = 0,


        //在这前面加//
        TIMER_MAX,
    }

    public class LogicTimer
	{
		private long nElapse = 1000;
		private int nEventID = 0;
		private long nBeginTime = 0;
		private bool bValid = true;
        private int mPos = 0;
		private _OnTimer handler = null;

		public long Elapse
		{
			set{
				nElapse = value;
			}
			
			get
			{
				return nElapse;
			}
		}
		
		public int EventID
		{
			set {
				nEventID = value;
			}
			
			get{
				return nEventID;
			}
		}
		
		public long BeginTime
		{
			set{
				nBeginTime = value;
			}
			
			get{
				return nBeginTime;
			}
		}
		
		public bool Valid
		{
			set{
				bValid = value;
			}
			
			get{
				return bValid;
			}
		}
		public _OnTimer Handler
		{
			set { handler = value;}
			get { return handler;}
		}

        public int Pos
        {
            set
            {
                mPos = value;
            }

            get
            {
                return mPos;
            }
        }

	}




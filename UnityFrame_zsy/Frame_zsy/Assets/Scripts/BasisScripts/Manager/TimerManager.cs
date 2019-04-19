using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour {



    static public void SetTimer(TimerID nEvent, int nElapse, _OnTimer handler)
    {
        mSetTimer(nEvent, nElapse, handler);
    }
   
    static public LogicTimer SetTimer(int nElapse, _OnTimer handler)
    {
        return mSetTimerEx(nElapse, handler);
    }


    static public Action<TimerID, int, _OnTimer> mSetTimer;

    static public Func<int, _OnTimer, LogicTimer> mSetTimerEx;

    //void KillCurTimer()
    static public Action KillCurTimer;

    //public void KillTimer( TimerID nEvent )
    static public Action<TimerID> KillTimer;

    //public LogicTimer GetTimer( int nEvent )
    static public Func<int, LogicTimer> GetTimer;

    //public LogicTimer ModifyTimerElapse( int nEvent, int elapse )
    static public Func<int, int, LogicTimer> ModifyTimerElapse;

    //public LogicTimer ResetBeginTime ( int nEvent )
    static public Func<int, LogicTimer> ResetBeginTime;

    //public LogicTimer CurTimer
    static public Func<LogicTimer> CurTimer;

    static public Func<int> GetCount;
}

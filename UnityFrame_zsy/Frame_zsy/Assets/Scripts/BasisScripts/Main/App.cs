using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * 此类为方便调用管理类
 ***/
public class App : BaseClass<App> {








    /// <summary>
    /// 事件管理类
    /// </summary>
	public static EventMgrHelper EventMgrHelper
    {
        get { return EventMgrHelper.Ins; }
    }

    /// <summary>
    /// ui控制管理
    /// </summary>
    public static ControllerManager ControllerManager
    {
        get { return ControllerManager.Ins; }
    }

    /// <summary>
    /// ui管理
    /// </summary>
    public static ViewManager ViewManager
    {
        get { return ViewManager.Ins; }
    }


    /// <summary>
    /// 初始化
    /// </summary>
    public void Init()
    {
        EventMgrHelper.Init();



    }




}

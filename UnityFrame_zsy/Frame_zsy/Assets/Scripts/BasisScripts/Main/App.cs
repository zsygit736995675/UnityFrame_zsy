using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * 此类为方便调用管理类
 ***/
public class App : BaseClass<App> {


    private bool useLocalConfig=true;
    public bool UseLocalConfig
    {
        set { useLocalConfig = value; }
        get { return useLocalConfig; }
    }


    /// <summary>
    /// 事件管理类
    /// </summary>
	public static EventMgrHelper EventMgrHelper
    {
        get { return EventMgrHelper.Ins; }
    }

    /// <summary>
    /// 控制管理
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
    /// 通讯管理
    /// </summary>
    public static NoticeManager NoticeManager
    {
        get { return NoticeManager.Ins; }
    }

    /// <summary>
    /// ab资源管理
    /// </summary>
    public static ResMgr ResManager
    {
        get { return ResMgr.Ins; }
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public void Init()
    {
        EventMgrHelper.Init();
        ControllerManager.RegisterAllController();
        NoticeManager.Init();
      
    }

    /// <summary>
    /// update
    /// </summary>
    public void Update()
    {
      
    }



}

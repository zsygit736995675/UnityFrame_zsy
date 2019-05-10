using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

    [SerializeField]
    private int localVersion = 1; //版本号

    [SerializeField]
    public bool UseLocalConfig;//是否使用本地表格

    private void Awake()
    {
      
        App.Ins.UseLocalConfig = UseLocalConfig;
        App.Ins.Init();
        VersionManager.Ins.Update(localVersion, LoadCallback);
    }

    /// <summary>
    /// 版本更新结束
    /// </summary>
    private void LoadCallback()
    {
        Debug.Log("LoadCallback");
    }

    // Use this for initialization
    void Start () {
        
        
	}
	
	// Update is called once per frame
	void Update () {
        App.Ins.Update();
	}

    private void OnApplicationFocus(bool focus)
    {
        if (!App.EventMgrHelper.isInit)
            return;

        if (focus)
        {
            Logger.Log("OnApplicationFocus  回到游戏");
        }
        else
        {
            Logger.Log("OnApplicationFocus  离开游戏");
        }
    }
    
    private void OnApplicationPause(bool Pause)
    {
        if (Pause)
        {
            Logger.Log("OnApplicationPause  回到游戏");
        }
        else
        {
            Logger.Log("OnApplicationPause  离开游戏");
        }
    }
}

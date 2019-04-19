using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

    

	// Use this for initialization
	void Start () {

        ConfFact.Register();
        Logger.Log(StrConfig.GetConfig(960).str);
	}
	
	// Update is called once per frame
	void Update () {
		
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

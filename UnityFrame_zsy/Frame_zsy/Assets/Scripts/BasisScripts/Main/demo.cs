using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class demo : MonoBehaviour {



     void Awake()
    {
#if !UNITY_EDITOR
        this.enabled = false;
        return;
#endif
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 100, 40), "测试按钮"))
        {
            
            this.enabled = false;
        }
        return;
    }








}

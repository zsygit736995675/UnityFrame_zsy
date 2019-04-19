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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : BaseController {


    public GameController()
    {
        RegisterEvent(EventDef.Test);

    }

    public override void HandleEvent(EventDef ev, LogicEvent le)
    {
        base.HandleEvent(ev, le);

        if (ev == EventDef.Test)
        {
            Logger.Log("..............Test");

        }

    }

}

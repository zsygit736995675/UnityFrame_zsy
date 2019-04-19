using UnityEngine;
using System;

namespace ClientCore
{
	class MsgHandlerFact
	{
	    public static void Build(MsgMgr mgr)
	    {
			% for cls in class_list:
	  		mgr.RegisterMsgHandleCallback( MsgDef.msgid_${cls}, ${cls}Handler.HandleMsg );
			% endfor
	    }
	}
}
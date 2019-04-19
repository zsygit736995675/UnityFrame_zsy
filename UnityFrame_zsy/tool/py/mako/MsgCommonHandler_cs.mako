	partial class ${class_name}
	{
	    public static void HandleMsg( MsgWrapper _msg ) {
			${msg_name} msg = _msg.ParseTo<${msg_name}>();
			${class_name}.HandleMsg(msg);
	    }
	}


    public partial class ${class_name} : MsgHelper
    {
        protected ${msg_name} msg;

        public ${class_name}()
        {
            msg = new ${msg_name}();
            opcode = MsgDef.msgid_${msg_name};
        }

        public void Send()
        {
			Send(msg);
        }
    }


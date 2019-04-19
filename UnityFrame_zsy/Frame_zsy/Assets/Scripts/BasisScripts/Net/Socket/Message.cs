using System;
using Share;

namespace Net
{
    public abstract class Message: Marshal
    {
        protected Session session = null;

	    public void sendResponse(Message msg)
	    {
		    if (this.session == null)
			    throw new Exception("not associate with a session");

		    this.session.Send(msg);
	    }

	    public void setSession(Session session)
	    {
		    this.session = session;
	    }

	    public Session getSession()
	    {
            return this.session;
	    }

	    public void dispatch()
	    {
		    this.session.Manager.dispatchMessage(this.session, this);
	    }

	    public abstract int getType();

	    public abstract void handle();

        public abstract Octets marshal(Octets oc);

        public abstract Octets unmarshal(Octets oc);
    }
}

using System;
using System.Net.Sockets;
using Share;

namespace Net
{
    public sealed class Session
    {
        private static readonly AtomicLong nextId = new AtomicLong(1);

        public readonly long id = nextId.GetAndAdd(1);
        private readonly Octets ibuffer = null;
        private readonly Socket socket;
        private readonly Manager manager;
        private readonly AtomicInteger closed = new AtomicInteger(0);

        public Session(Manager manager, Socket socket)
        {
            this.manager = manager;
            this.socket = socket;
            this.ibuffer = new Octets(manager.IBufferCapacity);
        }

        public Manager Manager
        {
            get { return manager; }
        }

        public void Connect(string host, int port)
        {
            this.socket.BeginConnect(host, port, new AsyncCallback(OnConnectResult), null);
        }

        public void Send(Message message)
        {
            try
            {
                Octets oc = new Octets();
                oc.push(message.getType());
                oc.push(0); // message size, push later
                int oldSize = oc.Size;
                oc.push(message);
                int size = oc.Size - oldSize;
                oc.push_rollback(size + 4);
                oc.push(size); // message size, push now
                oc.push_count(size);

                socket.BeginSend(oc.getBytes(), 0, oc.Size, SocketFlags.None, new AsyncCallback(OnSendResult), null);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError(e);
                Close();
            }
        }

        public void Close()
        {
            if (!closed.CompareAndSet(0, 1))
                return;

            try
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch
            {
            }

            try
            {
                manager.DelSession(this);
            }
            catch
            {
            }
        }

        private void Decode()
        {
            while (true)
            {
                if (ibuffer.Size < 8)
                    break;

                int type = ibuffer.pop_int();
                int size = ibuffer.pop_int();

                if (size > ibuffer.Capacity - 8)
                    throw new Exception("message size error: " + size);

                if (size <= ibuffer.Size)
                {
                    Message msg = manager.CreateMessage(type);
                    if (msg == null)
                    {
                        manager.onUnknownMessage(this, type, size, ibuffer);
                        continue;
                    }

                    msg.unmarshal(ibuffer);
                    msg.setSession(this);

                    try
                    {
                        msg.dispatch();
                    }
                    catch
                    {
                    }
                }
                else
                {
                    ibuffer.pop_rollback(8);
                    break;
                }
            }
        }

        private void OnConnectResult(IAsyncResult result)
        {
            try
            {
                socket.EndConnect(result);
                manager.AddSession(this);
                int offset, size;
                byte[] bytes = ibuffer.getBytesForWrite(out offset, out size);
                socket.BeginReceive(bytes, offset, size, SocketFlags.None, new AsyncCallback(onReceiveResult), null);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError(e);
                Close();
            }
        }

        private void onReceiveResult(IAsyncResult result)
        {
            try
            {
                int count = socket.EndReceive(result);
                if (count == 0)
                {
                    Close();
                    return;
                }

                ibuffer.push_count(count);
                Decode();
                int offset, size;
                byte[] bytes = ibuffer.getBytesForWrite(out offset, out size);
                socket.BeginReceive(bytes, offset, size, SocketFlags.None, new AsyncCallback(onReceiveResult), null);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError(e);
                Close();
            }
        }

        private void OnSendResult(IAsyncResult result)
        {
            try
            {
                socket.EndSend(result);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError(e);
                Close();
            }
        }
    }
}

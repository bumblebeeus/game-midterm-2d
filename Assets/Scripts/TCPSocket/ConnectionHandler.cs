using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace Server
{
    public class ConnectionHandler
    {
        private Socket _handle;
        private Thread _thread;
        private CommonSocket _cmSocket;
        private static Mutex _mut = new Mutex();
        public ConnectionHandler(CommonSocket cmSocket)
        {
            _handle = null;
            _cmSocket = cmSocket;
        }

        public void CreateConnection()
        {
            _thread = new Thread(() =>
            {
                var t = _cmSocket.GetHandle();
                _mut.WaitOne();
                _handle = t;
                _mut.ReleaseMutex();
            });
            _thread.Start();
        }

        // User's responsibility for Connection checking!
        public bool CheckConnection()
        {
            _mut.WaitOne();
            var result = _handle != null && _handle.Connected;
            _mut.ReleaseMutex();
            return result;
        }

        public void ShutdownConnection()
        {
            _mut.WaitOne();
            _handle.Shutdown(SocketShutdown.Both);
            _handle = null;
            _mut.ReleaseMutex();
        }

        public void ResetConnection()
        {
            ShutdownConnection();
            CreateConnection();
        }

        public void SendBytes(byte[] data)
        {
            _mut.WaitOne();
            _handle.Send(data);
            _mut.ReleaseMutex();
        }

        public byte[] RecvBytes()
        {
            _mut.WaitOne();
            var data = new byte[1024];
            _handle.Receive(data);
            _mut.ReleaseMutex();
            return data;
        }
    }
}
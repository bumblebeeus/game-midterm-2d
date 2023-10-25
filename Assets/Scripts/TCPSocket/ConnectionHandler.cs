using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using Unity.VisualScripting;

namespace Server
{
    public class ConnectionHandler
    {
        private Socket handle;
        private Thread thread;
        private CommonSocket cmSocket;
        private static Mutex _mut = new Mutex();
        public ConnectionHandler(CommonSocket cmSocket)
        {
            handle = null;
            this.cmSocket = cmSocket;
        }

        public void CreateConnection()
        {
            thread = new Thread(() =>
            {
                var t = cmSocket.GetHandle();
                _mut.WaitOne();
                handle = t;
                _mut.ReleaseMutex();
            });
            thread.Start();
        }

        // User's responsibility for Connection checking!
        public bool CheckConnection()
        {
            _mut.WaitOne();
            var result = handle != null && handle.Connected;
            _mut.ReleaseMutex();
            return result;
        }

        public void ShutdownConnection()
        {
            _mut.WaitOne();
            handle.Shutdown(SocketShutdown.Both);
            handle = null;
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
            handle.Send(data);
            _mut.ReleaseMutex();
        }

        public byte[] RecvBytes()
        {
            _mut.WaitOne();
            var data = new byte[1024]; // Vulnerable to manual crafted packet
            var len = handle.Receive(data);
            _mut.ReleaseMutex();
            Array.Resize(ref data, len);
            return data;
        }
    }
}
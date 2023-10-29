using System.Net;
using System.Net.Sockets;
namespace Server
{
    public class CommonSocket
    {
        private Socket _skHandle;
        private SkType _type;
        private IPEndPoint _endpoint;
        public CommonSocket(string ip, string port, SkType type = SkType.Listen)
        {
            _type = type;
            _skHandle = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);
            _endpoint = new IPEndPoint(IPAddress.Parse(ip), int.Parse(port));
            Initialize();
        }

        private void Initialize()
        {
            if (_type != SkType.Listen) return;
            _skHandle.Bind(_endpoint);
            _skHandle.Listen(1); // Allow only 1 connection a time
        }

        public Socket GetHandle()
        {
            return (_type == SkType.Listen ? AcceptSocket() : ConnectSocket());
        }

        public Socket AcceptSocket()
        {
            return _skHandle.Accept();
        }

        public Socket ConnectSocket()
        {
            _skHandle.Connect(_endpoint);
            return _skHandle;
        }

        public void Shutdown()
        {
            _skHandle.Shutdown(SocketShutdown.Both);
        }
    }
}
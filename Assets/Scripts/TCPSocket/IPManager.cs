using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Cryptography;

namespace Server
{
    public class IpManager
    {
        private static Random _r;
        public static bool CheckConectivity()
        {
            return System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
        }
        
        // Use with caution! (only first interface is used)
        public static string GetIPv4()
        {
            if (!CheckConectivity()) return null;
            string firstIp = null;
            firstIp = Dns.GetHostEntry(Dns.GetHostName())
                .AddressList
                .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork)?
                .ToString();

            return firstIp;
        }

        public static string GetRandomPort()
        {
            if (_r == null)
            {
                _r = new Random(DateTime.Now.Second);
            }
            return _r.Next(9000, 12000).ToString();
        }
    }
}
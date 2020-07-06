using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace SimpleMsul
{
    public class LocalAddressProvider
    {
        public IPAddress GetLocalIPAddress(IPAddress ForAddress)
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());

            return host.AddressList
                       .Where(ip => ip.AddressFamily == AddressFamily.InterNetwork)
                       .OrderByDescending(ip => Similarity(ip, ForAddress))
                       .FirstOrDefault()
                   ?? throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        private static int Similarity(IPAddress Ip, IPAddress ForAddress)
        {
            var a = BitConverter.ToUInt32(Ip.GetAddressBytes(),         0);
            var b = BitConverter.ToUInt32(ForAddress.GetAddressBytes(), 0);

            int i;
            for (i = 0; i < 32; i++)
                if ((a & (1 << i)) != (b & (1 << i)))
                    return i;

            return i;
        }
    }
}
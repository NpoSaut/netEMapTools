using System.Net;
using Communications.Can;
using MapViewer.Emulation.Blok.Can.UdpCan;

namespace MapViewer.Emulation.Blok.Can
{
    public class UdpCanPortHandlerProvider : ICanPortHandlerProvider
    {
        public ICanPortHandler OpenPort()
        {
            return new Handler(new UdpCanPort(new IPEndPoint(IPAddress.Loopback, 0),
                new IPEndPoint(IPAddress.Loopback, 50326)));
        }

        private class Handler : ICanPortHandler
        {
            public Handler(ICanPort Port)
            {
                this.Port = Port;
            }

            public void Dispose()
            {
            }

            public ICanPort Port { get; private set; }
        }
    }
}
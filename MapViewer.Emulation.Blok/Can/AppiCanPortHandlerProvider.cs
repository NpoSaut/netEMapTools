using System;
using System.Linq;
using Communications;
using Communications.Appi.Devices;
using Communications.Appi.Factories;
using Communications.Appi.StaticFactory;
using Communications.Can;
using Communications.Ports.Factories;
using Communications.Ports.Tokens;

namespace MapViewer.Emulation.Blok.Can
{
    public class AppiCanPortHandlerProvider : ICanPortHandlerProvider
    {
        public ICanPortHandler OpenPort()
        {
            var token = Appi.Any(AppiLine.Can1, AppiStandLine.CanA).Open().GetAwaiter().GetResult();
            return new PortTokenToPortHandlerProxy(token);
        }

        private class PortTokenToPortHandlerProxy : ICanPortHandler
        {
            private readonly IPortToken<ICanPort> _token;

            public PortTokenToPortHandlerProxy(IPortToken<ICanPort> token)
            {
                _token = token;
            }

            public void Dispose()
            {
                _token.Dispose();
            }

            public ICanPort Port => _token.Port;
        }
    }
}
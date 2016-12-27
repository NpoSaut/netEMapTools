using System;
using Communications.Appi;
using Communications.Appi.Devices;
using MapViewer.Emulation.Blok.Can;
using MapViewer.Emulation.Wheels;

namespace MapViewer.Emulation.Blok.Wheels
{
    public class CanWheelFactory : IWheelFactory
    {
        private readonly ICanPortHandlerProvider _canPortHandlerProvider;
        public CanWheelFactory(ICanPortHandlerProvider CanPortHandlerProvider) { _canPortHandlerProvider = CanPortHandlerProvider; }

        public IWheel GetWheel()
        {
            throw new NotImplementedException();
            //AppiDev appiDevice = _canPortHandlerProvider.OpenPort().Port;
            //return new CanWheel(appiDevice.CanPorts[AppiLine.Can1]);
        }
    }
}

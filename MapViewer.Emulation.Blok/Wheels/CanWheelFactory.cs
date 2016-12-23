using Communications.Appi;
using MapViewer.Emulation.Blok.Can;
using MapViewer.Emulation.Wheels;

namespace MapViewer.Emulation.Blok.Wheels
{
    public class CanWheelFactory : IWheelFactory
    {
        private readonly IAppiDeviceFactory _appiDeviceFactory;
        public CanWheelFactory(IAppiDeviceFactory AppiDeviceFactory) { _appiDeviceFactory = AppiDeviceFactory; }

        public IWheel GetWheel()
        {
            AppiDev appiDevice = _appiDeviceFactory.GetDevice().Dev;
            return new CanWheel(appiDevice.CanPorts[AppiLine.Can1]);
        }
    }
}

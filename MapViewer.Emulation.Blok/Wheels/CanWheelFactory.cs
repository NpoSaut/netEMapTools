using Communications.Appi;
using EMapNavigator.Can;
using MapViewer.Emulation.Wheels;

namespace MapViewer.Emulation.Blok.Wheels
{
    public class CanWheelFactory : IWheelFactory
    {
        private readonly IAppiDeviceFactory _appiDeviceFactory;
        public CanWheelFactory(IAppiDeviceFactory AppiDeviceFactory) { _appiDeviceFactory = AppiDeviceFactory; }

        public IWheel GetWheel()
        {
            AppiDev appiDevice = _appiDeviceFactory.GetDevice();
            return new CanWheel(appiDevice.CanPorts[AppiLine.Can1]);
        }
    }
}

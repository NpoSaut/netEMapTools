using Communications.Appi;
using EMapNavigator.Can;

namespace EMapNavigator.Emulation
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

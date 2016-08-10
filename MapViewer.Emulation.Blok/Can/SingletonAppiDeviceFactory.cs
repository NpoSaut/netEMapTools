using System;
using System.Linq;
using Communications;
using Communications.Appi;
using Communications.Appi.Winusb;

namespace MapViewer.Emulation.Blok.Can
{
    public class SingletonAppiDeviceFactory : IAppiDeviceFactory, IDisposable
    {
        private readonly Lazy<AppiDev> _lazyDev;

        public SingletonAppiDeviceFactory() { _lazyDev = new Lazy<AppiDev>(OpenDevice); }

        public AppiDev GetDevice() { return _lazyDev.Value; }

        private AppiDev OpenDevice()
        {
            AppiDev appiDevice = WinusbAppiDev.GetDevices().First().OpenDevice();
            appiDevice.CanPorts[AppiLine.Can1].BaudRate = BaudRates.CBR_100K;
            appiDevice.CanPorts[AppiLine.Can2].BaudRate = BaudRates.CBR_100K;
            return appiDevice;
        }

        public void Dispose()
        {
            if (_lazyDev.IsValueCreated)
                _lazyDev.Value.Dispose();
        }
    }
}

using System;
using System.Linq;
using Communications;
using Communications.Appi;
using Communications.Appi.Winusb;

namespace MapViewer.Emulation.Blok.Can
{
    public class SingletonAppiDeviceFactory : IAppiDeviceFactory, IDisposable
    {
        private readonly object _appiLocker = new object();
        private AppiDev _appiDev;
        private int _refCounter;

        public IAppiHandler GetDevice()
        {
            lock (_appiLocker)
            {
                if (_appiDev == null)
                    _appiDev = OpenDevice();
                _refCounter++;
                var handler = new Handler(_appiDev);
                handler.Disposed +=
                    (s, e) =>
                    {
                        lock (_appiLocker)
                        {
                            _refCounter--;
                            if (_refCounter == 0)
                            {
                                _appiDev.Dispose();
                                _appiDev = null;
                            }
                        }
                    };
                return handler;
            }
        }

        public void Dispose()
        {
            if (_appiDev != null)
                _appiDev.Dispose();
        }

        private AppiDev OpenDevice()
        {
            AppiDev appiDevice = WinusbAppiDev.GetDevices().First().OpenDevice();
            appiDevice.CanPorts[AppiLine.Can1].BaudRate = BaudRates.CBR_100K;
            appiDevice.CanPorts[AppiLine.Can2].BaudRate = BaudRates.CBR_100K;
            return appiDevice;
        }

        private class Handler : IAppiHandler
        {
            public Handler(AppiDev Dev) { this.Dev = Dev; }
            public void Dispose() { OnDisposed(); }
            public AppiDev Dev { get; private set; }
            public event EventHandler Disposed;

            protected virtual void OnDisposed()
            {
                EventHandler handler = Disposed;
                if (handler != null) handler(this, EventArgs.Empty);
            }
        }
    }
}

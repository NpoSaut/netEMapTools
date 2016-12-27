using System;
using System.Linq;
using Communications;
using Communications.Appi.Devices;
using Communications.Appi.Factories;
using Communications.Can;

namespace MapViewer.Emulation.Blok.Can
{
    public class AppiCanPortHandlerProvider : ICanPortHandlerProvider, IDisposable
    {
        private readonly IAppiFactory<AppiLine> _appiFactory;
        private readonly object _appiLocker = new object();

        private AppiDevice<AppiLine> _appiDev;
        private int _refCounter;
        public AppiCanPortHandlerProvider(IAppiFactory<AppiLine> AppiFactory) { _appiFactory = AppiFactory; }

        public ICanPortHandler OpenPort()
        {
            lock (_appiLocker)
            {
                if (_appiDev == null)
                    _appiDev = OpenDevice();
                _refCounter++;
                var handler = new Handler(_appiDev.CanPorts[AppiLine.Can1]);
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

        private AppiDevice<AppiLine> OpenDevice()
        {
            IAppiDeviceInfo slot = _appiFactory.EnumerateDevices().First();
            AppiDevice<AppiLine> dev = _appiFactory.OpenDevice(slot);

            dev.CanPorts[AppiLine.Can1].Options.BaudRate = BaudRates.CBR_100K;
            dev.CanPorts[AppiLine.Can2].Options.BaudRate = BaudRates.CBR_100K;
            return dev;
        }

        private class Handler : ICanPortHandler
        {
            public Handler(ICanPort Port) { this.Port = Port; }
            public void Dispose() { OnDisposed(); }
            public ICanPort Port { get; private set; }
            public event EventHandler Disposed;

            protected virtual void OnDisposed()
            {
                EventHandler handler = Disposed;
                if (handler != null) handler(this, EventArgs.Empty);
            }
        }
    }
}

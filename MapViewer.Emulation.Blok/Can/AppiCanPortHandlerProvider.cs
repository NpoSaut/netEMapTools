using System;
using System.Collections.Generic;
using System.Linq;
using Communications.Appi.Devices;
using Communications.Appi.Factories;
using Communications.Can;

namespace MapViewer.Emulation.Blok.Can
{
    public class AppiCanPortHandlerProvider : ICanPortHandlerProvider, IDisposable
    {
        private readonly IAppiFactory<AppiLine> _appiFactory;
        private readonly object _appiLocker = new object();
        private readonly IDictionary<AppiLine, int> _baudRates;
        private readonly AppiLine _line;

        private AppiDevice<AppiLine> _appiDev;
        private int _refCounter;

        public AppiCanPortHandlerProvider(IAppiFactory<AppiLine> AppiFactory, AppiLine Line, IDictionary<AppiLine, int> BaudRates)
        {
            _appiFactory = AppiFactory;
            _line = Line;
            _baudRates = BaudRates;
        }

        public ICanPortHandler OpenPort()
        {
            lock (_appiLocker)
            {
                if (_appiDev == null)
                    _appiDev = OpenDevice();
                _refCounter++;
                var handler = new Handler(_appiDev.CanPorts[_line]);
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

            foreach (var baudRate in _baudRates)
                dev.CanPorts[baudRate.Key].Options.BaudRate = baudRate.Value;
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

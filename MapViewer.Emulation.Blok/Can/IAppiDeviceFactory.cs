using System;
using Communications.Appi;

namespace MapViewer.Emulation.Blok.Can
{
    public interface IAppiDeviceFactory
    {
        IAppiHandler GetDevice();
    }

    public interface IAppiHandler : IDisposable
    {
        AppiDev Dev { get; }
    }
}

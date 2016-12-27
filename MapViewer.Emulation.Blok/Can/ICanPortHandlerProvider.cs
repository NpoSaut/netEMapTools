using System;
using Communications.Can;

namespace MapViewer.Emulation.Blok.Can
{
    public interface ICanPortHandlerProvider
    {
        ICanPortHandler GetDevice();
    }

    public interface ICanPortHandler : IDisposable
    {
        ICanPort Port { get; }
    }
}

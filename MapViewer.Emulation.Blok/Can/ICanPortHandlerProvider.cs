using System;
using Communications.Can;

namespace MapViewer.Emulation.Blok.Can
{
    public interface ICanPortHandlerProvider
    {
        ICanPortHandler OpenPort();
    }

    public interface ICanPortHandler : IDisposable
    {
        ICanPort Port { get; }
    }
}

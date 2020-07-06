using System;
using MapViewer.Emulation.Msul.Encoding;

namespace MapViewer.Emulation.Msul.Emit
{
    public interface IMsulEmitter
    {
        IDisposable Emit(IObservable<MsulData> Data);
    }
}
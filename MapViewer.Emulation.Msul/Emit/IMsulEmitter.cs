using System;

namespace MapViewer.Emulation.Msul.Emit
{
    public interface IMsulEmitter
    {
        IDisposable Emit(IObservable<MsulMessage> Messages);
    }
}
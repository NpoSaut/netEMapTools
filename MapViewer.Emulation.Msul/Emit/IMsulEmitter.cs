using System;

namespace MapViewer.Emulation.Msul.Emit
{
    public interface IMsulEmitter
    {
        IObservable<MsulMessage> Emit(IObservable<MsulMessage> Messages);
    }
}
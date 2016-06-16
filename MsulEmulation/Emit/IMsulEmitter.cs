using System;

namespace MsulEmulation.Emit
{
    public interface IMsulEmitter
    {
        IObservable<MsulMessage> Emit(IObservable<MsulMessage> Messages);
    }
}
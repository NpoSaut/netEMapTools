using System;

namespace MapViewer.Emulation.Msul.Emit
{
    public interface IMsulEmitterProvider
    {
        IObservable<IMsulEmitter> LeftEmitter { get; }
        IObservable<IMsulEmitter> RightEmitter { get; }
    }
}

using System;

namespace MapViewer.Emulation.Msul.Emit
{
    public static class MsulEmitterHelper
    {
        public static IDisposable EmitOn(this IObservable<MsulMessage> Messages, IMsulEmitter Emitter) { return Emitter.Emit(Messages); }
    }
}

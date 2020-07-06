using System;
using MapViewer.Emulation.Msul.Encoding;

namespace MapViewer.Emulation.Msul.Emit
{
    public static class MsulEmitterHelper
    {
        public static IDisposable EmitOn(this IObservable<MsulData> Data, IMsulEmitter Emitter) { return Emitter.Emit(Data); }
    }
}

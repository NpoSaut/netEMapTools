using System;

namespace MapViewer.Emulation.Msul.Emit
{
    public static class MsulEmitterHelper
    {
        public static IObservable<MsulMessage> EmitOn(this IObservable<MsulMessage> Messages, IMsulEmitter Emitter)
        {
            if (Emitter != null)
                return Emitter.Emit(Messages);
            return
                Messages;
        }
    }
}

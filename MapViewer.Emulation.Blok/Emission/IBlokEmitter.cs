using System;

namespace MapViewer.Emulation.Blok.Emission
{
    public interface IBlokEmitter
    {
        IObservable<NavigationInformation> Emit(IObservable<NavigationInformation> Navigation);
    }

    public static class BlokEmitterHelper
    {
        public static IObservable<NavigationInformation> EmitThorough(this IObservable<NavigationInformation> Navigation, IBlokEmitter Emitter)
        {
            return Emitter.Emit(Navigation);
        }
    }
}

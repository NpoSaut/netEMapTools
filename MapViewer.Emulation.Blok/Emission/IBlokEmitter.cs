using System;

namespace MapViewer.Emulation.Blok.Emission
{
    public interface IBlokEmitter : IDisposable
    {
        void Emit(IObservable<NavigationInformation> Navigation);
    }
}
